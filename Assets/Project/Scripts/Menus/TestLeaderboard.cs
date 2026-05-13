using UnityEngine;

public class TestLeaderboard : MonoBehaviour
{
    public LeaderboardManager leaderboard;

    void Start()
    {
        StartCoroutine(leaderboard.SendScore("JogadorTeste00001", 1400));
        StartCoroutine(leaderboard.GetTopScores());
    }
}