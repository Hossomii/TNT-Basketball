/*
Responsabilidade:
Gerenciar:
- Pontuação
- Combo
- Multiplicador
- Multiplicador externo (buff TNT)

Regras:
- Good = 1 ponto
- Perfect = 3 pontos
- Miss = reseta combo
- Combo:
  0~4 = 1x
  5~9 = 2x
  10+ = 3x

- Buff TNT:
  multiplica o multiplicador final
  podendo chegar até 6x
*/

using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [Header("Score")]
    public float score = 0f;

    [Header("Combo")]
    public int combo = 0;

    [Header("Multipliers")]
    public float comboMultiplier = 1f;
    public float externalMultiplier = 1f;

    [Header("Limits")]
    public float maxFinalMultiplier = 6f;

    [Header("UI")]
    public MultiplierUI multiplierUI;

    [Header("Debug")]
    public bool enableLogs = true;

    private void Start()
    {
        RefreshMultiplierUI();
    }

    public void ApplyShotResult(ShotEvaluator.ShotResult result)
    {
        float basePoints = GetBasePoints(result);

        HandleCombo(result);
        UpdateComboMultiplier();

        float finalMultiplier = GetFinalMultiplier();

        float pointsToAdd = basePoints * finalMultiplier;

        score += pointsToAdd;

        RefreshMultiplierUI();

        if (enableLogs)
        {
            Debug.Log(
                $"[{result}] " +
                $"Base: {basePoints} | " +
                $"Combo: {combo} | " +
                $"ComboMult: {comboMultiplier}x | " +
                $"External: {externalMultiplier}x | " +
                $"Final: {finalMultiplier}x | " +
                $"Added: {pointsToAdd} | " +
                $"Score: {score}"
            );
        }
    }

    private float GetBasePoints(ShotEvaluator.ShotResult result)
    {
        switch (result)
        {
            case ShotEvaluator.ShotResult.Perfect:
                return 3f;

            case ShotEvaluator.ShotResult.Good:
                return 1f;

            case ShotEvaluator.ShotResult.Miss:
                return 0f;
        }

        return 0f;
    }

    private void HandleCombo(ShotEvaluator.ShotResult result)
    {
        switch (result)
        {
            case ShotEvaluator.ShotResult.Perfect:
            case ShotEvaluator.ShotResult.Good:
                combo++;
                break;

            case ShotEvaluator.ShotResult.Miss:
                combo = 0;
                break;
        }
    }

    private void UpdateComboMultiplier()
    {
        if (combo >= 10)
        {
            comboMultiplier = 3f;
        }
        else if (combo >= 5)
        {
            comboMultiplier = 2f;
        }
        else
        {
            comboMultiplier = 1f;
        }
    }

    public float GetFinalMultiplier()
    {
        return Mathf.Clamp(
            comboMultiplier * externalMultiplier,
            1f,
            maxFinalMultiplier
        );
    }

    public void SetExternalMultiplier(float value)
    {
        externalMultiplier = value;

        RefreshMultiplierUI();
    }

    private void RefreshMultiplierUI()
    {
        if (multiplierUI == null)
            return;

        multiplierUI.UpdateMultiplier(GetFinalMultiplier());
    }

    public void ResetScore()
    {
        score = 0f;
        combo = 0;

        comboMultiplier = 1f;
        externalMultiplier = 1f;

        RefreshMultiplierUI();
    }

    public void SaveLastScore()
    {
        PlayerPrefs.SetInt("LastScore", Mathf.RoundToInt(score));
        PlayerPrefs.Save();

        Debug.Log("Último score salvo: " + score);
    }
}