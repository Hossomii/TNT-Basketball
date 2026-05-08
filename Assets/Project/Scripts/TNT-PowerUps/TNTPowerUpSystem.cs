/*
Responsabilidade:
Executar os efeitos das latas TNT.

Latas:
0 - Aumenta zona Perfect
1 - Congela timer
2 - Dobra pontuação
*/

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
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

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
        }
    }

    private IEnumerator GreenZoneBoostRoutine()
    {
        if (zoneRandomizer != null)
            zoneRandomizer.SetPerfectZoneBoost(true, boostedPerfectZoneSize);

        if (zoneUI != null)
            zoneUI.SetBoostVisual(true);

        yield return new WaitForSeconds(greenZoneBoostDuration);

        if (zoneRandomizer != null)
            zoneRandomizer.SetPerfectZoneBoost(false, boostedPerfectZoneSize);

        if (zoneUI != null)
            zoneUI.SetBoostVisual(false);

        activeRoutine = null;
    }

    private IEnumerator TimePauseRoutine()
    {
        if (timerSystem != null)
            timerSystem.SetTimePaused(true);

        if (timerUI != null)
            timerUI.SetFrozenVisual(true);

        yield return new WaitForSeconds(timePauseDuration);

        if (timerSystem != null)
            timerSystem.SetTimePaused(false);

        if (timerUI != null)
            timerUI.SetFrozenVisual(false);

        activeRoutine = null;
    }

    private IEnumerator DoubleScoreRoutine()
    {
        if (scoreSystem != null)
            scoreSystem.SetExternalMultiplier(doubleScoreMultiplier);

        if (multiplierUI != null)
            multiplierUI.SetDoubleScoreVisual(true);

        yield return new WaitForSeconds(doubleScoreDuration);

        if (scoreSystem != null)
            scoreSystem.SetExternalMultiplier(1f);

        if (multiplierUI != null)
            multiplierUI.SetDoubleScoreVisual(false);

        activeRoutine = null;
    }
}