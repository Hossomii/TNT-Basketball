using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Supabase")]
    public string supabaseUrl = "https://qlxesskphwzpxyjnhpju.supabase.co";
    public string anonKey = "sb_publishable_-H64CpcBFTvgu8tmpdCXLQ_2Ddn6a0c";

    [Header("Ranking UI")]
    public Transform contentParent;
    public GameObject rankingItemPrefab;

    [Header("Player Rank UI")]
    public TMP_Text playerRankText;

    public IEnumerator SendScore(string playerName, int score)
    {
        string url = $"{supabaseUrl}/rest/v1/rpc/submit_score";

        string json =
            $"{{\"p_player_name\":\"{playerName}\",\"p_score\":{score}}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", "Bearer " + anonKey);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao enviar score: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Score enviado/atualizado com sucesso!");
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
            ShowRanking(request.downloadHandler.text);
        }
    }

    public IEnumerator GetPlayerRank(string playerName, int playerScore)
    {
        string url = $"{supabaseUrl}/rest/v1/leaderboard?select=score&score=gt.{playerScore}";

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", "Bearer " + anonKey);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao buscar posição do jogador: " + request.downloadHandler.text);
        }
        else
        {
            ScoreEntry[] scoresAbove = JsonHelper.FromJson<ScoreEntry>(request.downloadHandler.text);

            int rank = scoresAbove.Length + 1;

            if(playerRankText != null)
            {
                playerRankText.text =
                    $"Seu ranking: #{rank}\n" +
                    $"Nome: {playerName}\n" +
                    $"Score: {playerScore}";
            }

            Debug.Log($"Seu ranking: #{rank} | Nome: {playerName} | Score: {playerScore}");
        }
    }

    private void ShowRanking(string json)
    {
        ScoreEntry[] scores = JsonHelper.FromJson<ScoreEntry>(json);

        if(contentParent == null || rankingItemPrefab == null)
        {
            Debug.LogError("Content Parent ou Ranking Item Prefab não foi conectado no Inspector.");
            return;
        }

        foreach(Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < scores.Length; i++)
        {
            GameObject item = Instantiate(rankingItemPrefab, contentParent);

            RankingItemUI itemUI = item.GetComponent<RankingItemUI>();

            if(itemUI != null)
            {
                itemUI.Setup(i + 1, scores[i].player_name, scores[i].score);
            }
        }
    }
}

[System.Serializable]
public class ScoreEntry
{
    public string player_name;
    public int score;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"array\":" + json + "}";

        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);

        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}