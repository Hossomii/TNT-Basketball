/*
Responsabilidade:
Controlar input do jogador e fluxo do arremesso.

Fluxo:
- Clique
- Avalia resultado
- Toca animação
- Toca áudio
- Atualiza score
- Atualiza energia TNT
- Randomiza zonas
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

    private void Update()
    {
        if (!CanShoot())
            return;

        HandleInput();
    }

    private bool CanShoot()
    {
        if (timerSystem != null && timerSystem.isGameOver)
            return false;

        if (isResolvingShot)
            return false;

        return true;
    }

    private void HandleInput()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(ResolveShotRoutine());
        }
    }

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
        {
            feedbackUI.Show(result);
        }

        if (scoreSystem != null)
        {
            scoreSystem.ApplyShotResult(result);
        }

        HandleTNT(result);

        if (result != ShotEvaluator.ShotResult.Miss)
        {
            zoneRandomizer?.TryRandomizeZones();
        }

        isResolvingShot = false;
    }

    private void HandleTNT(ShotEvaluator.ShotResult result)
    {
        if (tntSystem == null)
            return;

        if (result == ShotEvaluator.ShotResult.Miss)
        {
            tntSystem.ApplyMissPenalty();
        }
        else
        {
            tntSystem.AddEnergy(1);
        }
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