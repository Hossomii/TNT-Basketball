using System.Collections;
using UnityEngine;

public class TNTPowerUpSystem : MonoBehaviour
{
    [Header("References")]
    public ZoneRandomizer zoneRandomizer;
    public ZoneUI zoneUI;
    public TimerSystem timerSystem;
    public TimerUI timerUI;
    public ScoreSystem scoreSystem;
    public MultiplierUI multiplierUI;

    [Header("Ball Effects")]
    public BallVisualEffects ballVisualEffects;

    [Header("Durations")]
    public float greenZoneBoostDuration = 2f;
    public float timePauseDuration = 3f;
    public float doubleScoreDuration = 5f;

    [Header("Power Values")]
    public float boostedPerfectZoneSize = 0.18f;
    public float doubleScoreMultiplier = 2f;

    private Coroutine activeRoutine;

    public float GetPowerUpDuration(int index)
    {
        switch (index)
        {
            case 0:
                return greenZoneBoostDuration;

            case 1:
                return timePauseDuration;

            case 2:
                return doubleScoreDuration;

            default:
                return 0f;
        }
    }

    public void ActivatePowerUp(int index)
    {
        if (GameplayLockSystem.IsGameplayLocked)
            return;

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
            ResetAllPowerUps();
        }

        switch (index)
        {
            case 0:
                activeRoutine = StartCoroutine(GreenZoneBoostRoutine());
                break;

            case 1:
                activeRoutine = StartCoroutine(TimePauseRoutine());
                break;

            case 2:
                activeRoutine = StartCoroutine(DoubleScoreRoutine());
                break;

            default:
                Debug.LogWarning("PowerUp inexistente: " + index);
                break;
        }
    }

    private IEnumerator GreenZoneBoostRoutine()
    {
        if (zoneRandomizer != null)
            zoneRandomizer.SetPerfectZoneBoost(true, boostedPerfectZoneSize);

        if (zoneUI != null)
            zoneUI.SetBoostVisual(true);

        if (ballVisualEffects != null)
            ballVisualEffects.EnableLightningEffect();

        yield return new WaitForSeconds(greenZoneBoostDuration);

        if (zoneRandomizer != null)
            zoneRandomizer.SetPerfectZoneBoost(false, boostedPerfectZoneSize);

        if (zoneUI != null)
            zoneUI.SetBoostVisual(false);

        if (ballVisualEffects != null)
            ballVisualEffects.DisableAllEffects();

        activeRoutine = null;
    }

    private IEnumerator TimePauseRoutine()
    {
        if (timerSystem != null)
            timerSystem.SetTimePaused(true);

        if (timerUI != null)
            timerUI.SetFrozenVisual(true);

        if (ballVisualEffects != null)
            ballVisualEffects.EnableIceEffect();

        yield return new WaitForSecondsRealtime(timePauseDuration);

        if (timerSystem != null)
            timerSystem.SetTimePaused(false);

        if (timerUI != null)
            timerUI.SetFrozenVisual(false);

        if (ballVisualEffects != null)
            ballVisualEffects.DisableAllEffects();

        activeRoutine = null;
    }

    private IEnumerator DoubleScoreRoutine()
    {
        if (scoreSystem != null)
            scoreSystem.SetExternalMultiplier(doubleScoreMultiplier);

        if (multiplierUI != null)
            multiplierUI.SetDoubleScoreVisual(true);

        if (ballVisualEffects != null)
            ballVisualEffects.EnableFireEffect();

        yield return new WaitForSeconds(doubleScoreDuration);

        if (scoreSystem != null)
            scoreSystem.SetExternalMultiplier(1f);

        if (multiplierUI != null)
            multiplierUI.SetDoubleScoreVisual(false);

        if (ballVisualEffects != null)
            ballVisualEffects.DisableAllEffects();

        activeRoutine = null;
    }

    private void ResetAllPowerUps()
    {
        if (zoneRandomizer != null)
            zoneRandomizer.SetPerfectZoneBoost(false, boostedPerfectZoneSize);

        if (zoneUI != null)
            zoneUI.SetBoostVisual(false);

        if (timerSystem != null)
            timerSystem.SetTimePaused(false);

        if (timerUI != null)
            timerUI.SetFrozenVisual(false);

        if (scoreSystem != null)
            scoreSystem.SetExternalMultiplier(1f);

        if (multiplierUI != null)
            multiplierUI.SetDoubleScoreVisual(false);

        if (ballVisualEffects != null)
            ballVisualEffects.DisableAllEffects();
    }
}