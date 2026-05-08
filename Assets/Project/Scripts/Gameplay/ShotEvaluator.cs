/*
Responsabilidade:
Avaliar o resultado do arremesso baseado
na posição atual da barra.

Resultados:
- Miss
- Good
- Perfect

Usado por:
- InputHandler
- ZoneRandomizer
*/

using UnityEngine;

public class ShotEvaluator : MonoBehaviour
{
    [Header("References")]
    public PowerBar powerBar;

    [Header("Good Zone")]
    [Range(0f, 1f)] public float goodStart = 0.35f;
    [Range(0f, 1f)] public float goodEnd = 0.65f;

    [Header("Perfect Zone")]
    [Range(0f, 1f)] public float perfectStart = 0.45f;
    [Range(0f, 1f)] public float perfectEnd = 0.55f;

    [Header("Precision")]
    public float tolerance = 0.005f;

    [Header("Debug")]
    public bool enableLogs = false;

    public enum ShotResult
    {
        Miss,
        Good,
        Perfect
    }

    public ShotResult Evaluate()
    {
        if (powerBar == null)
        {
            Debug.LogWarning("PowerBar não encontrada.");
            return ShotResult.Miss;
        }

        float value = powerBar.value;

        if (enableLogs)
        {
            Debug.Log(
                $"Value: {value:F3} | " +
                $"Perfect: {perfectStart:F3}-{perfectEnd:F3} | " +
                $"Good: {goodStart:F3}-{goodEnd:F3}"
            );
        }

        if (IsInsidePerfectZone(value))
            return ShotResult.Perfect;

        if (IsInsideGoodZone(value))
            return ShotResult.Good;

        return ShotResult.Miss;
    }

    private bool IsInsidePerfectZone(float value)
    {
        return value >= perfectStart - tolerance &&
               value <= perfectEnd + tolerance;
    }

    private bool IsInsideGoodZone(float value)
    {
        return value >= goodStart - tolerance &&
               value <= goodEnd + tolerance;
    }

    /*
    Responsabilidade:
    Atualizar posições das zonas dinamicamente.
    */
    public void SetZones(
        float newGoodStart,
        float newGoodEnd,
        float newPerfectStart,
        float newPerfectEnd
    )
    {
        goodStart = newGoodStart;
        goodEnd = newGoodEnd;

        perfectStart = newPerfectStart;
        perfectEnd = newPerfectEnd;
    }
}