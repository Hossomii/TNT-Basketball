/*
Responsabilidade:
Controlar o tempo da partida.

Regras:
- O tempo começa em 60 segundos.
- Quando chega em 0, o jogo termina.
- Pode ser pausado temporariamente por powerups.
- Quando o jogo termina, a animação da bola para.
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
        timeRemaining = startTime;
        isGameOver = false;
        isPaused = false;

        if (ballAnimator != null)
            ballAnimator.speed = 1f;
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (isPaused)
            return;

        TickTimer();
    }

    private void TickTimer()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        timeRemaining = 0f;
        isGameOver = true;
        isPaused = false;

        if (ballAnimator != null)
            ballAnimator.speed = 0f;

        Debug.Log("FIM DE JOGO!");
    }

    public void SetTimePaused(bool value)
    {
        if (isGameOver)
            return;

        isPaused = value;
    }

    public void ResetTimer()
    {
        timeRemaining = startTime;
        isGameOver = false;
        isPaused = false;

        if (ballAnimator != null)
            ballAnimator.speed = 1f;
    }
}