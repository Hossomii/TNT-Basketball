using System.Collections;
using UnityEngine;
using TMPro;

public class GameStartCountdown : MonoBehaviour
{
    [Header("UI")]
    public GameObject overlay; // fundo preto
    public TextMeshProUGUI countdownText;

    [Header("Referências do jogo")]
    public TimerSystem timerSystem;
    public InputHandler inputHandler;
    public PowerBar powerBar;

    [Header("Referências da bola")]
    public Animator ballAnimator;

    private void Awake()
    {
        if (ballAnimator == null)
        {
            ballAnimator = GameObject.Find("Ball").GetComponent<Animator>();
        }
    }

    private void Start()
    {
        StartCoroutine(StartCountdownRoutine());
    }

    private IEnumerator StartCountdownRoutine()
    {
        // Ativa overlay
        if (overlay != null)
            overlay.SetActive(true);

        // Pausa sistemas
        if (timerSystem != null)
            timerSystem.SetTimePaused(true);

        if (inputHandler != null)
            inputHandler.enabled = false;

        if (powerBar != null)
            powerBar.enabled = false;

        if(ballAnimator != null)
            ballAnimator.speed = 0f;

        // Countdown 3 → 1
        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null)
                countdownText.text = i.ToString();

            yield return new WaitForSeconds(1f);
        }

        // "VAI!"
        if (countdownText != null)
            countdownText.text = "VAI!";

        yield return new WaitForSeconds(0.5f);

        // Desativa overlay
        if (overlay != null)
            overlay.SetActive(false);

        // Libera sistemas
        if (timerSystem != null)
            timerSystem.SetTimePaused(false);

        if (powerBar != null)
            powerBar.enabled = true;

        if (inputHandler != null)
            inputHandler.enabled = true;

        if (ballAnimator != null)
            ballAnimator.speed = 1f;
    }
}