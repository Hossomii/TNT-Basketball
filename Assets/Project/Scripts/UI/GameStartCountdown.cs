using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartCountdown : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject overlayObject;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameplayLockSystem gameplayLockSystem;

    [Header("Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.2f;

    [Header("Settings")]
    [SerializeField] private int startNumber = 3;
    [SerializeField] private float numberDuration = 1f;
    [SerializeField] private float goDuration = 0.5f;
    [SerializeField] private string goText = "VAI!";

    private void Awake()
    {
        if (gameplayLockSystem == null)
            gameplayLockSystem =
                FindFirstObjectByType<GameplayLockSystem>();
    }

    private void Start()
    {
        StartCoroutine(StartSequenceRoutine());
    }

    /*
    Responsabilidade:
    Executar:
    fade branco -> countdown -> gameplay
    */
    private IEnumerator StartSequenceRoutine()
    {
        gameplayLockSystem?.LockGameplay();

        if (overlayObject != null)
            overlayObject.SetActive(true);

        if (countdownText != null)
            countdownText.text = "";

        SetupFade();

        yield return null;

        yield return PlayFade();

        yield return PlayCountdown();

        if (overlayObject != null)
            overlayObject.SetActive(false);

        gameplayLockSystem?.UnlockGameplay();
    }

    /*
    Responsabilidade:
    Preparar a imagem branca do fade.
    */
    private void SetupFade()
    {
        if (fadeImage == null)
            return;

        fadeImage.gameObject.SetActive(true);

        fadeImage.raycastTarget = false;

        Color color = Color.white;
        color.a = 1f;

        fadeImage.color = color;

        fadeImage.canvasRenderer.SetAlpha(1f);
    }

    /*
    Responsabilidade:
    Fazer fade branco desaparecer suavemente.
    */
    private IEnumerator PlayFade()
    {
        if (fadeImage == null)
            yield break;

        fadeImage.CrossFadeAlpha(
            0f,
            fadeDuration,
            true
        );

        yield return new WaitForSecondsRealtime(fadeDuration);

        fadeImage.gameObject.SetActive(false);
    }

    /*
    Responsabilidade:
    Executar:
    3 -> 2 -> 1 -> VAI!
    */
    private IEnumerator PlayCountdown()
    {
        for (int i = startNumber; i > 0; i--)
        {
            if (countdownText != null)
                countdownText.text = i.ToString();

            AudioManager.Instance?.PlayCountdownNumber(i);

            yield return new WaitForSecondsRealtime(numberDuration);
        }

        if (countdownText != null)
            countdownText.text = goText;

        AudioManager.Instance?.PlayCountdownGo();

        yield return new WaitForSecondsRealtime(goDuration);
    }
}