/*
Responsabilidade:
Controlar posição e tamanho das zonas Good/Perfect.

Regras:
- Início da partida: zonas centralizadas.
- Após o tempo definido: zonas randomizam após acertos.
- PowerUp TNT pode aumentar temporariamente a zona Perfect.
*/

using UnityEngine;

public class ZoneRandomizer : MonoBehaviour
{
    [Header("References")]
    public ShotEvaluator shotEvaluator;
    public ZoneUI zoneUI;
    public TimerSystem timerSystem;

    [Header("Zone Sizes")]
    [Range(0f, 1f)] public float goodZoneSize = 0.5f;
    [Range(0f, 1f)] public float perfectZoneSize = 0.3f;

    [Header("Randomization")]
    public float randomizeAfterTime = 50f;

    [Header("TNT PowerUp")]
    public bool isPerfectBoostActive = false;
    [Range(0f, 1f)] public float boostedPerfectZoneSize = 0.18f;

    private float lastCenter = 0.5f;

    private void Start()
    {
        SetCenteredZones();
    }

    public void TryRandomizeZones()
    {
        if (ShouldKeepCentered())
        {
            SetCenteredZones();
            return;
        }

        RandomizeZones();
    }

    public void SetPerfectZoneBoost(bool active, float boostedSize)
    {
        isPerfectBoostActive = active;
        boostedPerfectZoneSize = Mathf.Clamp01(boostedSize);

        ApplyZones(lastCenter);
    }

    private bool ShouldKeepCentered()
    {
        return timerSystem != null &&
               timerSystem.timeRemaining > randomizeAfterTime;
    }

    private void SetCenteredZones()
    {
        lastCenter = 0.5f;
        ApplyZones(lastCenter);
    }

    private void RandomizeZones()
    {
        float safeHalfGood = Mathf.Clamp01(goodZoneSize) / 2f;

        lastCenter = Random.Range(
            safeHalfGood,
            1f - safeHalfGood
        );

        ApplyZones(lastCenter);
    }

    /*
    Responsabilidade:
    Aplicar valores normalizados das zonas na lógica e na UI.
    */
    private void ApplyZones(float center)
    {
        float safeGoodSize = Mathf.Clamp01(goodZoneSize);

        float activePerfectSize = isPerfectBoostActive
            ? boostedPerfectZoneSize
            : perfectZoneSize;

        activePerfectSize = Mathf.Clamp(
            activePerfectSize,
            0f,
            safeGoodSize
        );

        float halfGood = safeGoodSize / 2f;
        float halfPerfect = activePerfectSize / 2f;

        float goodStart = center - halfGood;
        float goodEnd = center + halfGood;

        float perfectStart = center - halfPerfect;
        float perfectEnd = center + halfPerfect;

        if (shotEvaluator != null)
        {
            shotEvaluator.SetZones(
                goodStart,
                goodEnd,
                perfectStart,
                perfectEnd
            );
        }

        if (zoneUI != null)
        {
            zoneUI.UpdateZones(
                goodStart,
                goodEnd,
                perfectStart,
                perfectEnd
            );
        }
    }
}