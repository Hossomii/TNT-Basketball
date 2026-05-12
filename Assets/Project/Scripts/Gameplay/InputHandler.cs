/*
Responsabilidade:
Controlar a entrada do jogador durante a gameplay.

Esse script detecta quando o jogador clica/toca para arremessar
e inicia o fluxo completo de resolução do arremesso.

Fluxo:
Input
-> ShotEvaluator
-> BallAnimationController
-> FeedbackUI
-> ScoreSystem
-> TNTSystem
-> ZoneRandomizer

Dependências:
- ShotEvaluator: avalia se foi Miss, Good ou Perfect
- BallAnimationController: toca animação da bola
- FeedbackUI: mostra BOM, PERFEITO ou ERROU
- ScoreSystem: aplica pontuação, combo e multiplicador
- TNTSystem: adiciona energia ou aplica penalidade
- ZoneRandomizer: muda as zonas após acerto
- TimerSystem: impede input após game over
- GameplayLockSystem: bloqueia input durante countdown/pause
- AudioManager: toca sons do arremesso e resultado
*/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("References")]
    public ShotEvaluator evaluator;
    public FeedbackUI feedbackUI;
    public ScoreSystem scoreSystem;
    public ZoneRandomizer zoneRandomizer;
    public TimerSystem timerSystem;
    public BallAnimationController ballAnimationController;
    public TNTSystem tntSystem;

    [Header("State")]
    public bool isResolvingShot = false;

    [Header("Debug")]
    public bool enableLogs = false;

    private Coroutine shotRoutine;

    private void Update()
    {
        if (!CanShoot())
            return;

        HandleInput();
    }

    /*
    Responsabilidade:
    Define se o jogador pode tentar arremessar agora.

    Bloqueia arremesso quando:
    - countdown/pause está ativo
    - jogo acabou
    - uma animação de arremesso ainda está resolvendo
    - ShotEvaluator não foi configurado
    */
    private bool CanShoot()
    {
        if (GameplayLockSystem.IsGameplayLocked)
            return false;

        if (timerSystem != null && timerSystem.isGameOver)
            return false;

        if (isResolvingShot)
            return false;

        if (evaluator == null)
            return false;

        return true;
    }

    /*
    Responsabilidade:
    Detectar input do jogador.

    Atualmente:
    - clique esquerdo do mouse

    Futuro:
    - toque na tela para mobile/WebGL
    */
    private void HandleInput()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryResolveShot();
        }

        // Suporte futuro para mobile/touch.
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            TryResolveShot();
        }
    }

    /*
    Responsabilidade:
    Iniciar a resolução do arremesso com segurança.

    Evita iniciar duas coroutines ao mesmo tempo.
    */
    private void TryResolveShot()
    {
        if (shotRoutine != null)
            return;

        shotRoutine = StartCoroutine(ResolveShotRoutine());
    }

    /*
    Responsabilidade:
    Resolver todo o fluxo de um arremesso.

    Ordem atual preservada:
    1. trava input
    2. avalia resultado
    3. toca som de arremesso
    4. toca animação da bola
    5. toca som do resultado
    6. mostra feedback
    7. aplica score
    8. aplica TNT
    9. randomiza zonas em caso de acerto
    10. libera input
    */
    private IEnumerator ResolveShotRoutine()
    {
        isResolvingShot = true;

        ShotEvaluator.ShotResult result = evaluator.Evaluate();

        if (enableLogs)
            Debug.Log($"Shot Result: {result}");

        PlayShootSound();

        if (ballAnimationController != null)
        {
            yield return StartCoroutine(
                ballAnimationController.PlayResultAnimation(result)
            );
        }

        PlayResultSound(result);

        if (feedbackUI != null)
            feedbackUI.Show(result);

        if (scoreSystem != null)
            scoreSystem.ApplyShotResult(result);

        HandleTNT(result);

        if (result != ShotEvaluator.ShotResult.Miss)
            zoneRandomizer?.TryRandomizeZones();

        isResolvingShot = false;
        shotRoutine = null;
    }

    /*
    Responsabilidade:
    Atualizar energia TNT baseada no resultado.
    */
    private void HandleTNT(ShotEvaluator.ShotResult result)
    {
        if (tntSystem == null)
            return;

        if (result == ShotEvaluator.ShotResult.Miss)
            tntSystem.ApplyMissPenalty();
        else
            tntSystem.AddEnergy(1);
    }

    private void PlayShootSound()
    {
        AudioManager.Instance?.PlayShoot();
    }

    private void PlayResultSound(ShotEvaluator.ShotResult result)
    {
        switch (result)
        {
            case ShotEvaluator.ShotResult.Perfect:
                AudioManager.Instance?.PlayPerfect();
                break;

            case ShotEvaluator.ShotResult.Good:
                AudioManager.Instance?.PlayHit();
                break;

            case ShotEvaluator.ShotResult.Miss:
                AudioManager.Instance?.PlayMiss();
                break;
        }
    }
}