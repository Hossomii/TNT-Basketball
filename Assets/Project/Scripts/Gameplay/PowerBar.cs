using UnityEngine;

public class PowerBar : MonoBehaviour
{
    [Header("Indicator Settings")]
    public float value = 0f;
    public float speed = 0.6f;

    [Header("Progressive Speed")]
    public TimerSystem timerSystem;

    public float startSpeed = 0.6f;
    public float speedIncreaseEvery10Seconds = 0.35f;
    public float maxSpeed = 3f;

    private bool goingRight = true;

    private void Update()
    {
        if (timerSystem != null && timerSystem.isGameOver)
            return;

        UpdateSpeedByTime();
        MovePointer();
    }

    private void UpdateSpeedByTime()
    {
        if (timerSystem == null) return;

        float elapsedTime = 60f - timerSystem.timeRemaining;
        int difficultyStep = Mathf.FloorToInt(elapsedTime / 10f);

        speed = startSpeed + (difficultyStep * speedIncreaseEvery10Seconds);

        if (speed > maxSpeed)
            speed = maxSpeed;
    }

    private void MovePointer()
    {
        if (goingRight)
        {
            value += speed * Time.deltaTime;

            if (value >= 1f)
            {
                value = 1f;
                goingRight = false;
            }
        }
        else
        {
            value -= speed * Time.deltaTime;

            if (value <= 0f)
            {
                value = 0f;
                goingRight = true;
            }
        }
    }
}