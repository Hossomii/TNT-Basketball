using System.Collections;
using UnityEngine;

public class RankingSceneController : MonoBehaviour
{
    public LeaderboardManager leaderboard;

    IEnumerator Start()
    {
        string playerName = PlayerPrefs.GetString("player_name", "Player");
        int playerScore = PlayerPrefs.GetInt("last_score", 0);

        yield return StartCoroutine(leaderboard.GetTopScores());
        yield return StartCoroutine(leaderboard.GetPlayerRank(playerName, playerScore));
    }
}