using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public ShotEvaluator evaluator;
    public FeedbackUI feedback;
    public ScoreSystem scoreSystem;
    public ZoneRandomizer zoneRandomizer;
    public TimerSystem timerSystem;
    // public EnergySystem energySystem;
    public BallAnimationController ballAnimationController;

    private bool isResolvingShot = false;

    private void Update()
    {
        if (timerSystem != null && timerSystem.isGameOver)
            return;

        if (isResolvingShot)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(ResolveShotRoutine());
        }
    }

    private IEnumerator ResolveShotRoutine()
    {
        isResolvingShot = true;

        var result = evaluator.Evaluate();

        if (ballAnimationController != null)
        {
            yield return StartCoroutine(ballAnimationController.PlayResultAnimation(result));
        }

        feedback.Show(result);
        scoreSystem.ApplyShotResult(result);

        if (result != ShotEvaluator.ShotResult.Miss)
        {   
            // if (energySystem != null)
            // energySystem.AddEnergy(1);

            zoneRandomizer.TryRandomizeZones();
        }

        isResolvingShot = false;
    }
}