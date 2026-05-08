using UnityEngine;

public class TestLeaderboard : MonoBehaviour
{
    public LeaderboardManager leaderboard;

    void Start()
    {
        StartCoroutine(leaderboard.SendScore("JogadorTeste", 1000));
        StartCoroutine(leaderboard.GetTopScores());
    }
}