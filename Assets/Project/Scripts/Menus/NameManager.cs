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

        int score = PlayerPrefs.GetInt("LastScore", 0);

        PlayerPrefs.SetString("player_name", playerName);
        PlayerPrefs.SetInt("last_score", score);
        PlayerPrefs.Save();

        StartCoroutine(SendScoreAndGoToRanking(playerName, score));
    }

    IEnumerator SendScoreAndGoToRanking(string playerName, int score)
    {
        yield return StartCoroutine(leaderboard.SendScore(playerName, score));

        SceneManager.LoadScene(5);
    }
}