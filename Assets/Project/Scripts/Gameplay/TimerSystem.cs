using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    public float timeRemaining = 60f;
    public bool isGameOver = false;
    public bool isPaused = false;
    public Animator ballAnimator;

    private void Update()
    {
        if (isGameOver) return;
        if (isPaused) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isGameOver = true;

            // Pausa a animação da bola
            if (ballAnimator != null)
                ballAnimator.speed = 0f;

            Debug.Log("FIM DE JOGO!");
        }
    }

    public void SetTimePaused(bool value)
    {
        isPaused = value;
    }
}