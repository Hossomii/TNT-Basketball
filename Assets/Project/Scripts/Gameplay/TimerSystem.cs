/*
Responsabilidade:
Controlar o tempo principal da partida.

Esse script gerencia:
- tempo restante
- estado de game over
- pausa temporária do tempo
- pausa da animação da bola quando necessário

Regras:
- o timer só conta quando a gameplay está liberada
- o timer para durante o countdown
- o timer para quando o jogo acaba
- o timer pode ser pausado pela lata azul
- ao chegar em 0, o jogo entra em Game Over

Dependências:
- GameplayLockSystem: bloqueia o timer durante countdown/pause global
- TNTPowerUpSystem: chama SetTimePaused() quando a lata azul ativa
- TimerUI: lê timeRemaining para atualizar o texto visual
- Ball Animator: pode ser pausado no countdown ou no game over
*/

using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    [Header("Timer")]
    public float startTime = 60f;
    public float timeRemaining = 60f;

    [Header("State")]
    public bool isGameOver = false;
    public bool isPaused = false;

    [Header("References")]
    public Animator ballAnimator;

    private void Start()
    {
        ResetTimer();
    }

    private void OnEnable()
    {
        GameplayLockSystem.OnGameplayLocked += UpdateBallAnimatorState;
        GameplayLockSystem.OnGameplayUnlocked += UpdateBallAnimatorState;
    }

    private void OnDisable()
    {
        GameplayLockSystem.OnGameplayLocked -= UpdateBallAnimatorState;
        GameplayLockSystem.OnGameplayUnlocked -= UpdateBallAnimatorState;
    }

    private void Update()
    {
        if (!CanTick())
            return;

        TickTimer();
    }

    /*
    Responsabilidade:
    Verificar se o timer pode contar agora.
    */
    private bool CanTick()
    {
        if (GameplayLockSystem.IsGameplayLocked)
            return false;

        if (isGameOver)
            return false;

        if (isPaused)
            return false;

        return true;
    }

    /*
    Responsabilidade:
    Reduzir o tempo da partida.
    */
    private void TickTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
            EndGame();
    }

    /*
    Responsabilidade:
    Encerrar a partida quando o tempo acaba.
    */
    private void EndGame()
    {
        timeRemaining = 0f;
        isGameOver = true;
        isPaused = false;

        UpdateBallAnimatorState();

        Debug.Log("FIM DE JOGO!");
    }

    /*
    Responsabilidade:
    Pausar ou retomar somente o tempo da partida.

    Usado principalmente pela lata azul.
    */
    public void SetTimePaused(bool value)
    {
        if (isGameOver)
            return;

        isPaused = value;
    }

    /*
    Responsabilidade:
    Resetar o timer para uma nova partida.
    */
    public void ResetTimer()
    {
        timeRemaining = startTime;
        isGameOver = false;
        isPaused = false;

        UpdateBallAnimatorState();
    }

    /*
    Responsabilidade:
    Pausar ou retomar animação da bola conforme o estado global.

    A bola pausa quando:
    - gameplay está bloqueada
    - jogo acabou
    */
    private void UpdateBallAnimatorState()
    {
        if (ballAnimator == null)
            return;

        ballAnimator.speed =
            GameplayLockSystem.IsGameplayLocked || isGameOver
                ? 0f
                : 1f;
    }
}