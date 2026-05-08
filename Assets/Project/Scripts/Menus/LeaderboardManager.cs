using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Supabase")]
    public string supabaseUrl = "https://qlxesskphwzpxyjnhpju.supabase.co";
    public string anonKey = "sb_publishable_-H64CpcBFTvgu8tmpdCXLQ_2Ddn6a0c";

    public IEnumerator SendScore(string playerName, int score)
    {
        string url = $"{supabaseUrl}/rest/v1/leaderboard";

        string json = $"{{\"player_name\":\"{playerName}\",\"score\":{score}}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", "Bearer " + anonKey);
        request.SetRequestHeader("Prefer", "return=minimal");

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao enviar score: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Score enviado com sucesso!");
        }
    }

    public IEnumerator GetTopScores()
    {
        string url = $"{supabaseUrl}/rest/v1/leaderboard?select=player_name,score&order=score.desc&limit=10";

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", "Bearer " + anonKey);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao buscar ranking: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Ranking recebido:");
            Debug.Log(request.downloadHandler.text);
        }
    }
}