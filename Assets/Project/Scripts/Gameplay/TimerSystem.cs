/*
Responsabilidade:
Controlar o tempo da partida.

Como funciona:
- Começa com 60 segundos.
- Diminui o tempo enquanto o jogo está ativo.
- Permite aplicar penalidade de tempo.
- Quando chega em 0, encerra a partida.

Depende de:
- InputHandler para receber penalidade por erro.
*/

using UnityEngine;

public class TimerSystem : MonoBehaviour
{
    public float timeRemaining = 60f;
    public bool isGameOver = false;

    private void Update()
    {
        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isGameOver = true;

            Debug.Log("FIM DE JOGO!");
        }
    }

    public void ApplyPenalty(float penalty)
    {
        if (isGameOver) return;

        timeRemaining -= penalty;

        if (timeRemaining < 0f)
            timeRemaining = 0f;
    }
}