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

        scoreText.text = $"Score: {scoreSystem.score:0.0}";
        comboText.text = $"Combo: {scoreSystem.combo}";
        multiplierText.text = $"Multi: {scoreSystem.multiplier:0.00}x";
    }
}