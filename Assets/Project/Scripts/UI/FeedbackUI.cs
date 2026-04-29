using UnityEngine;
using TMPro;

public class FeedbackUI : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Show(ShotEvaluator.ShotResult result)
    {
        switch (result)
        {
            case ShotEvaluator.ShotResult.Perfect:
                text.text = "PERFEITO!";
                text.color = Color.green;
                break;

            case ShotEvaluator.ShotResult.Good:
                text.text = "BOA!";
                text.color = Color.yellow;
                break;

            case ShotEvaluator.ShotResult.Miss:
                text.text = "ERROU";
                text.color = Color.red;
                break;
        }
    }
}

/*
Responsabilidade:
Mostrar feedback visual ao jogador após cada tentativa.

Como funciona:
- Recebe o resultado do ShotEvaluator.
- Atualiza o texto e cor na tela:
  - Perfect
  - Good
  - Miss

Depende de:
- InputHandler (quem chama)

Observação:
Apenas visual, não altera lógica do jogo.
*/