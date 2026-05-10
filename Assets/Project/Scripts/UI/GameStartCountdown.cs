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
            gameplayLockSystem = FindFirstObjectByType<GameplayLockSystem>();
    }

    private void Start()
    {
        StartCountdown();
    }

    public void StartCountdown()
    {
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        gameplayLockSystem?.LockGameplay();

        if (overlayObject != null)
            overlayObject.SetActive(true);

        for (int i = startNumber; i > 0; i--)
        {
            if (countdownText != null)
                countdownText.text = i.ToString();

            yield return new WaitForSecondsRealtime(numberDuration);
        }

        if (countdownText != null)
            countdownText.text = goText;

        yield return new WaitForSecondsRealtime(goDuration);

        if (overlayObject != null)
            overlayObject.SetActive(false);

        gameplayLockSystem?.UnlockGameplay();

        countdownRoutine = null;
    }
}