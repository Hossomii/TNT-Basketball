/*
Responsabilidade:
Controla a posição das zonas Good e Perfect na barra.

Regras:
- Nos primeiros 10 segundos de partida, as zonas ficam fixas no centro.
- Quando o tempo chega em 50s ou menos, as zonas passam a mudar aleatoriamente após cada acerto.
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

    private void Start()
    {
        SetCenteredZones();
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

    private void ApplyZones(float center)
    {
        float halfGood = goodZoneSize / 2f;
        float halfPerfect = perfectZoneSize / 2f;

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