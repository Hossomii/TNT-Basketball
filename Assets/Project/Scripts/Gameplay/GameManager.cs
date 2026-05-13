/*
Responsabilidade:
Controlar o estado global da partida.

Esse script:
- detecta o fim do timer
- salva a pontuação final
- mostra tela simples de fim de jogo
- toca apito e torcida
- carrega a cena de ranking após um pequeno delay

Dependências:
- TimerSystem
- ScoreSystem
- EndGameOverlay
- AudioManager
*/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public TimerSystem timerSystem;
    public ScoreSystem scoreSystem;

    [Header("End Game UI")]
    public GameObject endGameOverlay;

    [Header("Scene")]
    public string nextSceneName = "NameScene";

    [Header("Timing")]
    public float endGameDelay = 1.5f;

    private bool gameEnded = false;

    private void Update()
    {
        if (gameEnded)
            return;

        if (timerSystem != null && timerSystem.isGameOver)
            StartCoroutine(EndGameRoutine());
    }

    private IEnumerator EndGameRoutine()
    {
        gameEnded = true;

        GameplayLockSystem lockSystem = FindFirstObjectByType<GameplayLockSystem>();
        lockSystem?.LockGameplay();

        if (scoreSystem != null)
            scoreSystem.SaveLastScore();

        if (endGameOverlay != null)
            endGameOverlay.SetActive(true);

        AudioManager.Instance?.PlayFinalWhistle();
        AudioManager.Instance?.PlayCrowdCheer();

        yield return new WaitForSecondsRealtime(endGameDelay);

        SceneManager.LoadScene(nextSceneName);
    }
}