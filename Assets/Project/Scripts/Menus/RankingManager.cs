using UnityEngine;
using TMPro;

using Unity.Services.Core;
using Unity.Services.Authentication;

using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

using System.Threading.Tasks;

public class RankingManager : MonoBehaviour
{
    public Transform content;
    public GameObject rowPrefab;

    public TMP_Text rankText;

    async void Start()
    {
        await InitializeServices();

        await LoadLeaderboard();

        await GetPlayerRank();
    }

    async Task InitializeServices()
    {
        await UnityServices.InitializeAsync();

        if(!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance
                .SignInAnonymouslyAsync();
        }

        Debug.Log("Unity Services inicializado!");
    }

    async Task LoadLeaderboard()
    {
        var scores =
            await LeaderboardsService.Instance
            .GetScoresAsync("HighScore");

        foreach(var entry in scores.Results)
        {
            GameObject row =
                Instantiate(rowPrefab, content);

            TMP_Text[] texts =
                row.GetComponentsInChildren<TMP_Text>();

            texts[0].text =
                "#" + (entry.Rank + 1);

            texts[1].text =
                entry.PlayerName;

            texts[2].text =
                entry.Score.ToString();
        }
    }

    async Task GetPlayerRank()
    {
        var playerScore =
            await LeaderboardsService.Instance
            .GetPlayerScoreAsync("HighScore");

        rankText.text =
            "Seu Rank: #" +
            (playerScore.Rank + 1);
    }
}