using UnityEngine;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Login")]
    public TMP_InputField nameInput;
    public GameObject loginPanel;

    [Header("Leaderboard")]
    public Transform contentParent;
    public GameObject rowPrefab;

    [Header("Player")]
    public TMP_Text rankText;
    public TMP_Text scoreText;

    private string leaderboardID = "HighScore";

    async void Start()
    {
        await InitializeUGS();
    }

    async Task InitializeUGS()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log("Login realizado!");

        CheckPlayerName();

        await LoadLeaderboard();
    }

    // NICKNAME
    void CheckPlayerName()
    {
        string savedName = PlayerPrefs.GetString("PlayerName", "");

        if(string.IsNullOrEmpty(savedName))
        {
            loginPanel.SetActive(true);
        }
        else
        {
            loginPanel.SetActive(false);
        }
    }

    public async void SavePlayerName()
    {
        string playerName = nameInput.text;

        PlayerPrefs.SetString("PlayerName", playerName);

        await AuthenticationService.Instance.UpdatePlayerNameAsync(
            playerName
        );

        loginPanel.SetActive(false);

        Debug.Log("Nome do jogador salvo: " + playerName);
    }



    // ENVIAR SCORE
    public async void SendScore(int score)
    {
        scoreText.text = "Score: " + score;

        await LeaderboardsService.Instance.AddPlayerScoreAsync(
            leaderboardID,
            score
        );

        Debug.Log("Score enviado!");

        await LoadLeaderboard();

        await GetPlayerRank();
    }

    // TOP PLAYERS
    async Task LoadLeaderboard()
    {
        foreach(Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        var scores = await LeaderboardsService.Instance.GetScoresAsync(
            leaderboardID
        );

        foreach(var entry in scores.Results)
        {
            GameObject row = Instantiate(rowPrefab, contentParent);

            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

            texts[0].text = "#" + (entry.Rank + 1);

            string playerName = entry.PlayerName;

            if(string.IsNullOrEmpty(playerName))
            {
                playerName = "Player";
            }

            texts[1].text = playerName;

            texts[2].text = entry.Score.ToString();
        }
    }

    // RANK DO PLAYER
    async Task GetPlayerRank()
    {
        var playerEntry =
            await LeaderboardsService.Instance.GetPlayerScoreAsync(
                leaderboardID
            );

        rankText.text =
            "Seu Rank: #" + (playerEntry.Rank + 1);
    }
}