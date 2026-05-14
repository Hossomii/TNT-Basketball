using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class NameManager : MonoBehaviour
{

    public TMP_InputField nameInput;
    public LeaderboardManager leaderboard;

    public void SaveName()
    {
        string playerName = nameInput.text;

        if(string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Player";
        }

        string oldPlayerName = PlayerPrefs.GetString("player_name", "");

        int score = PlayerPrefs.GetInt("LastScore", 0);

        PlayerPrefs.SetString("player_name", playerName);
        PlayerPrefs.SetInt("last_score", score);
        PlayerPrefs.Save();

        StartCoroutine(SendScoreAndGoToRanking(oldPlayerName, playerName, score));
    }

    IEnumerator SendScoreAndGoToRanking(string oldPlayerName, string playerName, int score)
    {
        if(!string.IsNullOrEmpty(oldPlayerName))
        {
            yield return StartCoroutine(leaderboard.DeleteOldScores(oldPlayerName));
        }

        yield return StartCoroutine(leaderboard.DeleteOldScores(playerName));

        yield return StartCoroutine(leaderboard.SendScore(playerName, score));

        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(4);
    }
}