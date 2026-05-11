using UnityEngine;

public class PowerBar : MonoBehaviour
{
    [Header("Value")]
    [Range(0f, 1f)]
    public float value = 0f;

    [Header("Speed")]
    public float speed = 0.6f;

    [Header("Difficulty")]
    public float startSpeed = 0.6f;
    public float speedIncreaseEvery10Seconds = 0.35f;
    public float maxSpeed = 3f;

    [Header("References")]
    public TimerSystem timerSystem;

    [Header("State")]
    public bool goingRight = true;

    [Header("Debug")]
    public bool enableLogs = false;

    private void Start()
    {
        speed = startSpeed;
    }

    private void Update()
    {
        if (GameplayLockSystem.IsGameplayLocked)
            return;

        if (!CanMove())
            return;

        UpdateDifficulty();
        MovePointer();
    }

    private bool CanMove()
    {
        if (timerSystem == null)
            return true;

        if (timerSystem.isGameOver)
            return false;

        return true;
    }

    private void UpdateDifficulty()
    {
        if (timerSystem == null)
            return;

        float elapsedTime = timerSystem.startTime - timerSystem.timeRemaining;

        int difficultyStep = Mathf.FloorToInt(elapsedTime / 10f);

        speed = startSpeed + (difficultyStep * speedIncreaseEvery10Seconds);
        speed = Mathf.Clamp(speed, startSpeed, maxSpeed);

        if (enableLogs)
            Debug.Log($"PowerBar Speed: {speed}");
    }

    private void MovePointer()
    {
        float direction = goingRight ? 1f : -1f;

        value += direction * speed * Time.deltaTime;

        if (value >= 1f)
        {
            value = 1f;
            goingRight = false;
        }
        else if (value <= 0f)
        {
            value = 0f;
            goingRight = true;
        }
    }

    public void ResetBar()
    {
        value = 0f;
        speed = startSpeed;
        goingRight = true;
    }
}