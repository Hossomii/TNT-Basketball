/*
Responsabilidade:
Controlar a contagem inicial da partida.

Esse script:
- bloqueia a gameplay durante a contagem
- mostra o overlay de countdown
- toca os sons de 3, 2, 1 e VAI!
- libera a gameplay ao finalizar

Dependências:
- GameplayLockSystem: bloqueia/desbloqueia gameplay
- AudioManager: toca os sons da contagem
- TMP_Text: mostra os números na tela
*/

using System.Collections;
using TMPro;
using UnityEngine;

public class GameStartCountdown : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject overlayObject;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameplayLockSystem gameplayLockSystem;

    [Header("Settings")]
    [SerializeField] private int startNumber = 3;
    [SerializeField] private float numberDuration = 1f;
    [SerializeField] private float goDuration = 0.5f;
    [SerializeField] private string goText = "VAI!";

    private Coroutine countdownRoutine;

    private void Awake()
    {
        if (gameplayLockSystem == null)
            gameplayLockSystem =
                FindFirstObjectByType<GameplayLockSystem>();
    }

    private void Start()
    {
        StartCountdown();
    }

    /*
    Responsabilidade:
    Iniciar a contagem inicial.
    */
    public void StartCountdown()
    {
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine =
            StartCoroutine(CountdownRoutine());
    }

    /*
    Responsabilidade:
    Executar a sequência:
    3 -> 2 -> 1 -> VAI!
    */
    private IEnumerator CountdownRoutine()
    {
        gameplayLockSystem?.LockGameplay();

        if (overlayObject != null)
            overlayObject.SetActive(true);

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

        if (overlayObject != null)
            overlayObject.SetActive(false);

        gameplayLockSystem?.UnlockGameplay();

        countdownRoutine = null;
    }
}