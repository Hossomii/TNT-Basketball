/*
Responsabilidade:
Exibir na UI os valores atuais de pontuação, combo e multiplicador.

Como funciona:
- Lê os dados do ScoreSystem.
- Atualiza os textos na tela em tempo real.

Depende de:
- ScoreSystem

Observação:
Apenas visual. Não altera a lógica da pontuação.
*/

using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public ScoreSystem scoreSystem;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;

    private void Update()
    {
        if (scoreSystem == null)
            return;

        scoreText.text = ((int)scoreSystem.score).ToString("N0");
        multiplierText.text = scoreSystem.multiplier.ToString("F0") + "x";
        comboText.text = scoreSystem.combo.ToString();
    }
}