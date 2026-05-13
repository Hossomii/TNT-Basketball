/*
Responsabilidade:
Controlar a pontuação principal do jogador.

Esse script gerencia:
- score total
- combo atual
- multiplicador por combo
- multiplicador externo dos buffs TNT
- atualização visual do multiplicador
- salvamento do último score para o ranking

Regras atuais:
- Good = 1 ponto
- Perfect = 3 pontos
- Miss = 0 ponto e reseta combo

Combo:
- 0 até 4 acertos = 1x
- 5 até 9 acertos = 2x
- 10 ou mais acertos = 3x

Buff TNT:
- multiplica o multiplicador final
- exemplo: combo 3x + buff 2x = 6x

Dependências:
- ShotEvaluator: fornece o resultado do arremesso
- MultiplierUI: atualiza o visual do multiplicador
- TNTSystem / TNTPowerUpSystem: podem alterar externalMultiplier
- Ranking/GameManager: pode chamar SaveLastScore()
*/

using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [Header("Score")]
    public float score = 0f;

    [Header("Combo")]
    public int combo = 0;

    [Header("Points")]
    [SerializeField] private float goodPoints = 1f;
    [SerializeField] private float perfectPoints = 3f;

    [Header("Combo Rules")]
    [SerializeField] private int comboForDoubleMultiplier = 5;
    [SerializeField] private int comboForTripleMultiplier = 10;

    [Header("Multipliers")]
    public float comboMultiplier = 1f;
    public float externalMultiplier = 1f;

    [Header("Limits")]
    public float maxFinalMultiplier = 6f;

    [Header("UI")]
    public MultiplierUI multiplierUI;

    [Header("Debug")]
    public bool enableLogs = false;

    private void Start()
    {
        RefreshMultiplierUI();
    }

    public void ApplyShotResult(ShotEvaluator.ShotResult result)
    {
        float basePoints = GetBasePoints(result);

        UpdateCombo(result);
        UpdateComboMultiplier();

        float finalMultiplier = GetFinalMultiplier();
        float pointsToAdd = basePoints * finalMultiplier;

        score += pointsToAdd;

        RefreshMultiplierUI();

        if (enableLogs)
            LogScore(result, basePoints, finalMultiplier, pointsToAdd);
    }

    private float GetBasePoints(ShotEvaluator.ShotResult result)
    {
        switch (result)
        {
            case ShotEvaluator.ShotResult.Perfect:
                return perfectPoints;

            case ShotEvaluator.ShotResult.Good:
                return goodPoints;

            case ShotEvaluator.ShotResult.Miss:
                return 0f;

            default:
                return 0f;
        }
    }

    private void UpdateCombo(ShotEvaluator.ShotResult result)
    {
        if (result == ShotEvaluator.ShotResult.Miss)
        {
            combo = 0;
            return;
        }

        combo++;
    }

    private void UpdateComboMultiplier()
    {
        if (combo >= comboForTripleMultiplier)
        {
            comboMultiplier = 3f;
            return;
        }

        if (combo >= comboForDoubleMultiplier)
        {
            comboMultiplier = 2f;
            return;
        }

        comboMultiplier = 1f;
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
        externalMultiplier = Mathf.Max(1f, value);

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

    private void LogScore(
        ShotEvaluator.ShotResult result,
        float basePoints,
        float finalMultiplier,
        float pointsToAdd
    )
    {
        Debug.Log(
            $"ScoreSystem | Result: {result} | " +
            $"Base: {basePoints} | " +
            $"Combo: {combo} | " +
            $"ComboMult: {comboMultiplier}x | " +
            $"External: {externalMultiplier}x | " +
            $"Final: {finalMultiplier}x | " +
            $"Added: {pointsToAdd} | " +
            $"Score: {score}"
        );
    }

    public void SaveLastScore()
    {
        PlayerPrefs.SetInt(
            "LastScore",
            Mathf.RoundToInt(score)
        );

        PlayerPrefs.Save();

        if (enableLogs)
            Debug.Log("Último score salvo: " + score);
    }
}