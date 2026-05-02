/*
Responsabilidade:
Gerenciar pontuação, combo e multiplicador.

Como funciona:
- Good vale 1 ponto.
- Perfect vale 3 pontos.
- A cada acerto, aumenta o combo.
- Ao errar, zera o combo.
- Combo 5+ ativa multiplicador 2x.
- Combo 10+ ativa multiplicador 3x.

Depende de:
- InputHandler
*/

using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [Header("Score")]
    public float score = 0f;

    [Header("Combo")]
    public int combo = 0;
    public float multiplier = 1f;

    [Header("External Multiplier")]
        public float externalMultiplier = 1f;

    public void ApplyShotResult(ShotEvaluator.ShotResult result)
    {
        float basePoints = 0f;

        switch (result)
        {
            case ShotEvaluator.ShotResult.Perfect:
                basePoints = 3f;
                combo++;
                break;

            case ShotEvaluator.ShotResult.Good:
                basePoints = 1f;
                combo++;
                break;

            case ShotEvaluator.ShotResult.Miss:
                basePoints = 0f;
                combo = 0;
                break;
        }

        UpdateMultiplier();

        float pointsToAdd = basePoints * multiplier * externalMultiplier;
        score += pointsToAdd;

        Debug.Log($"Resultado: {result} | +{(int)pointsToAdd} pts | Score: {(int)score} | Combo: {combo} | Multi: {(int)multiplier}x");
    }

    private void UpdateMultiplier()
    {
        if (combo >= 10)
            multiplier = 3f;
        else if (combo >= 5)
            multiplier = 2f;
        else
            multiplier = 1f;
    }

    public void SetExternalMultiplier(float value)
    {
        externalMultiplier = value;
    }
}