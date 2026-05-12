/*
Responsabilidade:
Executar os efeitos das latas TNT.

Esse script NÃO controla energia e NÃO escolhe qual lata vem agora.
Ele apenas recebe o índice da lata e aplica o efeito correspondente.

PowerUps:
0 - Lata Roxa:
    aumenta temporariamente a zona Perfect

1 - Lata Azul:
    pausa temporariamente o timer

2 - Lata Laranja:
    dobra temporariamente a pontuação

Dependências:
- ZoneRandomizer: altera o tamanho da zona Perfect
- ZoneUI: muda o visual da zona quando está buffada
- TimerSystem: pausa e retoma o tempo
- TimerUI: muda o visual do timer congelado
- ScoreSystem: aplica multiplicador externo
- MultiplierUI: muda visual do multiplicador durante double score
- BallVisualEffects: ativa efeitos visuais na bola

Fluxo:
TNTSystem
-> ActivatePowerUp(index)
-> inicia coroutine do poder
-> espera duração
-> reseta efeito
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

        StopActivePowerUp();

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
                Debug.LogWarning($"TNTPowerUpSystem: PowerUp inexistente: {index}");
                break;
        }
    }

    private void StopActivePowerUp()
    {
        if (activeRoutine == null)
            return;

        StopCoroutine(activeRoutine);
        activeRoutine = null;

        ResetAllPowerUps();
    }

    private IEnumerator GreenZoneBoostRoutine()
    {
        SetGreenZoneBoost(true);
        ballVisualEffects?.EnableLightningEffect();

        yield return new WaitForSeconds(greenZoneBoostDuration);

        SetGreenZoneBoost(false);
        ballVisualEffects?.DisableAllEffects();

        activeRoutine = null;
    }

    private IEnumerator TimePauseRoutine()
    {
        SetTimePause(true);
        ballVisualEffects?.EnableIceEffect();

        yield return new WaitForSecondsRealtime(timePauseDuration);

        SetTimePause(false);
        ballVisualEffects?.DisableAllEffects();

        activeRoutine = null;
    }

    private IEnumerator DoubleScoreRoutine()
    {
        SetDoubleScore(true);
        ballVisualEffects?.EnableFireEffect();

        yield return new WaitForSeconds(doubleScoreDuration);

        SetDoubleScore(false);
        ballVisualEffects?.DisableAllEffects();

        activeRoutine = null;
    }

    private void SetGreenZoneBoost(bool isActive)
    {
        zoneRandomizer?.SetPerfectZoneBoost(isActive, boostedPerfectZoneSize);
        zoneUI?.SetBoostVisual(isActive);
    }

    private void SetTimePause(bool isActive)
    {
        timerSystem?.SetTimePaused(isActive);
        timerUI?.SetFrozenVisual(isActive);
    }

    private void SetDoubleScore(bool isActive)
    {
        float multiplier = isActive ? doubleScoreMultiplier : 1f;

        scoreSystem?.SetExternalMultiplier(multiplier);
        multiplierUI?.SetDoubleScoreVisual(isActive);
    }

    private void ResetAllPowerUps()
    {
        SetGreenZoneBoost(false);
        SetTimePause(false);
        SetDoubleScore(false);

        ballVisualEffects?.DisableAllEffects();
    }
}