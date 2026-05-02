using System.Collections;
using UnityEngine;

public class TNTPowerUpSystem : MonoBehaviour
{
    [Header("References")]
    public ZoneRandomizer zoneRandomizer;
    public TimerSystem timerSystem;
    public ScoreSystem scoreSystem;

    [Header("Durations")]
    public float greenZoneBoostDuration = 2f;
    public float timePauseDuration = 3f;
    public float doubleScoreDuration = 5f;

    public void ActivatePowerUp(int index)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(GreenZoneBoostRoutine());
                break;

            case 1:
                StartCoroutine(TimePauseRoutine());
                break;

            case 2:
                StartCoroutine(DoubleScoreRoutine());
                break;
        }
    }

    private IEnumerator GreenZoneBoostRoutine()
    {
        Debug.Log("PowerUp TNT: área verde aumentada");

        if (zoneRandomizer != null)
            zoneRandomizer.SetPerfectZoneBoost(true);

        yield return new WaitForSeconds(greenZoneBoostDuration);

        if (zoneRandomizer != null)
            zoneRandomizer.SetPerfectZoneBoost(false);
    }

    private IEnumerator TimePauseRoutine()
    {
        Debug.Log("PowerUp TNT: tempo pausado");

        if (timerSystem != null)
            timerSystem.SetTimePaused(true);

        yield return new WaitForSeconds(timePauseDuration);

        if (timerSystem != null)
            timerSystem.SetTimePaused(false);
    }

    private IEnumerator DoubleScoreRoutine()
    {
        Debug.Log("PowerUp TNT: pontuação dobrada");

        if (scoreSystem != null)
            scoreSystem.SetExternalMultiplier(2f);

        yield return new WaitForSeconds(doubleScoreDuration);

        if (scoreSystem != null)
            scoreSystem.SetExternalMultiplier(1f);
    }
}