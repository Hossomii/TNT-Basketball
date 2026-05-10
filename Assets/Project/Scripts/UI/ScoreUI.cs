/*
Responsabilidade:
Atualizar UI de score e combo.

Usado por:
- ScoreSystem
*/

using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    [Header("References")]
    public ScoreSystem scoreSystem;

    [Header("Texts")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    [Header("Labels")]
    public string comboPrefix = "COMBO ";

    private void Update()
    {
        if (scoreSystem == null)
            return;

        UpdateScore();
        UpdateCombo();
    }

    /*
    Responsabilidade:
    Atualizar texto da pontuação.
    */
    private void UpdateScore()
    {
        if (scoreText == null)
            return;

        scoreText.text =
            Mathf.RoundToInt(scoreSystem.score).ToString();
    }

    /*
    Responsabilidade:
    Atualizar texto do combo.
    */
    private void UpdateCombo()
    {
        if (comboText == null)
            return;

        comboText.text =
            $"{comboPrefix}{scoreSystem.combo}";
    }
}