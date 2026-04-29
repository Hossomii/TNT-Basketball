using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public ShotEvaluator evaluator;
    public FeedbackUI feedback;
    public ScoreSystem scoreSystem;
    public ZoneRandomizer zoneRandomizer;
    public TimerSystem timerSystem;

    private void Update()
    {
        if (timerSystem != null && timerSystem.isGameOver)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            var result = evaluator.Evaluate();

            feedback.Show(result);
            scoreSystem.ApplyShotResult(result);

            if (result != ShotEvaluator.ShotResult.Miss)
            {
                zoneRandomizer.TryRandomizeZones();
            }
        }
    }
}