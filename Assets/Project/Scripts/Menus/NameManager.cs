using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;

using System.Threading.Tasks;

public class NameManager : MonoBehaviour
{
    public TMP_InputField nameInput;

    async void Start()
    {
        await UnityServices.InitializeAsync();

        await AuthenticationService.Instance
            .SignInAnonymouslyAsync();
    }

    public async void SaveName()
    {
        string playerName = nameInput.text;

        await AuthenticationService.Instance
            .UpdatePlayerNameAsync(playerName);

        int score = PlayerPrefs.GetInt("LastScore");

        await LeaderboardsService.Instance
            .AddPlayerScoreAsync(
                "HighScore",
                score
            );

        SceneManager.LoadScene("RankingScene");
    }
}