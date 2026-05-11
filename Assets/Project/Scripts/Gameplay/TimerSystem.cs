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

        UpdateBallAnimatorState();
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
        if (GameplayLockSystem.IsGameplayLocked)
            return;

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
            EndGame();
    }

    private void EndGame()
    {
        timeRemaining = 0f;
        isGameOver = true;
        isPaused = false;

        UpdateBallAnimatorState();

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

        UpdateBallAnimatorState();
    }

    private void UpdateBallAnimatorState()
    {
        if (ballAnimator == null)
            return;

        if (GameplayLockSystem.IsGameplayLocked || isGameOver)
            ballAnimator.speed = 0f;
        else
            ballAnimator.speed = 1f;
    }
}