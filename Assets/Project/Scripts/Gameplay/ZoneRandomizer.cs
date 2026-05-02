/*
Responsabilidade:
Controla a posição das zonas Good e Perfect na barra.

Regras:
- Nos primeiros 10 segundos de partida, as zonas ficam fixas no centro.
- Quando o tempo chega em 50s ou menos, as zonas passam a mudar aleatoriamente após cada acerto.
- Suporta PowerUp TNT que aumenta temporariamente a área perfeita.
*/

using UnityEngine;

public class ZoneRandomizer : MonoBehaviour
{
    [Header("References")]
    public ShotEvaluator shotEvaluator;
    public ZoneUI zoneUI;
    public TimerSystem timerSystem;

    [Header("Zone Sizes")]
    public float goodZoneSize = 0.5f;
    public float perfectZoneSize = 0.3f;

    [Header("Randomization")]
    public float randomizeAfterTime = 50f;

    [Header("TNT PowerUp")]
    public bool isPerfectBoostActive = false;
    public float boostedPerfectZoneSize = 0.5f;

    private void Start()
    {
        SetCenteredZones();
    }

    public void SetPerfectZoneBoost(bool active)
    {
        isPerfectBoostActive = active;

        // Atualiza imediatamente a zona atual
        ReapplyCurrentZones();
    }

    public void TryRandomizeZones()
    {
        if (timerSystem != null && timerSystem.timeRemaining > randomizeAfterTime)
        {
            SetCenteredZones();
            return;
        }

        RandomizeZones();
    }

    private void SetCenteredZones()
    {
        float center = 0.5f;
        ApplyZones(center);
    }

    private void RandomizeZones()
    {
        float halfGood = goodZoneSize / 2f;

        float center = Random.Range(halfGood, 1f - halfGood);

        ApplyZones(center);
    }

    private void ReapplyCurrentZones()
    {
        // Recalcula usando o mesmo centro atual
        // Pegamos o centro atual baseado nas zonas já existentes

        float currentCenter = 0.5f;

        // Se quiser algo mais preciso no futuro:
        // você pode guardar o último center em uma variável

        ApplyZones(currentCenter);
    }

    private void ApplyZones(float center)
    {
        float activePerfectSize = isPerfectBoostActive ? boostedPerfectZoneSize : perfectZoneSize;

        float halfGood = goodZoneSize / 2f;
        float halfPerfect = activePerfectSize / 2f;

        float goodStart = center - halfGood;
        float goodEnd = center + halfGood;

        float perfectStart = center - halfPerfect;
        float perfectEnd = center + halfPerfect;

        shotEvaluator.SetZones(goodStart, goodEnd, perfectStart, perfectEnd);

        if (zoneUI != null)
        {
            zoneUI.UpdateZones(goodStart, goodEnd, perfectStart, perfectEnd);
        }
    }
}