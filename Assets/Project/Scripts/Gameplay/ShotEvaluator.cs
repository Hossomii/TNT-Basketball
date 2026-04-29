/*
Responsabilidade:
Avaliar se a posição da seta está em Miss, Good ou Perfect.

Como funciona:
- Lê o valor atual da seta no PowerBar.
- Compara com as zonas atuais:
  - Perfect
  - Good
  - Miss

Usado por:
- InputHandler
- ZoneRandomizer

Observação:
As zonas podem mudar de posição durante o jogo.
*/

using UnityEngine;

public class ShotEvaluator : MonoBehaviour
{
    public PowerBar powerBar;

    [Header("Zones")]
    [Range(0f, 1f)] public float goodStart = 0.35f;
    [Range(0f, 1f)] public float goodEnd = 0.65f;

    [Range(0f, 1f)] public float perfectStart = 0.45f;
    [Range(0f, 1f)] public float perfectEnd = 0.55f;

    [Header("Precision")]
    public float tolerance = 0.005f;

    public enum ShotResult
    {
        Miss,
        Good,
        Perfect
    }

    public ShotResult Evaluate()
    {
        float tolerance = 0.005f;
        float value = powerBar.value;

        Debug.Log($"Value: {value:F3} | Perfect: {perfectStart:F3}-{perfectEnd:F3} | Good: {goodStart:F3}-{goodEnd:F3}");

        if (value >= perfectStart - tolerance && value <= perfectEnd + tolerance)
            return ShotResult.Perfect;

        if (value >= goodStart - tolerance && value <= goodEnd + tolerance)
            return ShotResult.Good;

        return ShotResult.Miss;
    }

    public void SetZones(float newGoodStart, float newGoodEnd, float newPerfectStart, float newPerfectEnd)
    {
        goodStart = newGoodStart;
        goodEnd = newGoodEnd;
        perfectStart = newPerfectStart;
        perfectEnd = newPerfectEnd;
    }
}