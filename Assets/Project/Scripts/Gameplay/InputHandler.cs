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
    public BallAnimationController ballAnimationController;
    public TNTSystem tntSystem;

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

        AudioManager.Instance?.PlayShoot();

        if (ballAnimationController != null)
            yield return StartCoroutine(ballAnimationController.PlayResultAnimation(result));

        switch (result)
        {
            case ShotEvaluator.ShotResult.Perfect:
                AudioManager.Instance?.PlayPerfect();
                break;

            case ShotEvaluator.ShotResult.Good:
                AudioManager.Instance?.PlayHit();
                break;

            case ShotEvaluator.ShotResult.Miss:
                AudioManager.Instance?.PlayMiss();
                break;
        }

        feedback.Show(result);
        scoreSystem.ApplyShotResult(result);

        if (result != ShotEvaluator.ShotResult.Miss)
        {
            if (tntSystem != null)
                tntSystem.AddEnergy(1);

            zoneRandomizer.TryRandomizeZones();
        }

        isResolvingShot = false;
    }
}