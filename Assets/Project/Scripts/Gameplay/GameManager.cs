/*
Responsabilidade:
Controlar o estado global do jogo.

Como funciona:
- Recebe evento de fim de jogo do TimerSystem
- Ativa UI de fim de jogo
- Pode pausar sistemas futuramente

Depende de:
- TimerSystem
*/

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TimerSystem timerSystem;
    public GameObject gameOverUI;

    private bool gameEnded = false;

    private void Update()
    {
        if (gameEnded) return;

        if (timerSystem != null && timerSystem.isGameOver)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameEnded = true;

        Debug.Log("GAME OVER TRIGGERADO");

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }
}