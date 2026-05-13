using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Supabase")]
    public string supabaseUrl = "https://SEU_PROJETO.supabase.co";
    public string anonKey = "SUA_ANON_KEY";

    [Header("Ranking Prefab")]
    public GameObject rankingRowPrefab;
    public Transform rankingContent;

    [Header("Seu Ranking")]
    public TMP_Text myRankingText;

    public IEnumerator SendScore(string playerName, int score)
    {
        string url = $"{supabaseUrl}/rest/v1/leaderboard?select=id,player_name,score,created_at";

        string json = $"{{\"player_name\":\"{playerName}\",\"score\":{score}}}";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", "Bearer " + anonKey);
        request.SetRequestHeader("Prefer", "return=representation");

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
            Debug.LogError("Erro ao enviar score: " + request.downloadHandler.text);
        else
        {
            ScoreEntry[] insertedScore =
    JsonHelper.FromJson<ScoreEntry>(request.downloadHandler.text);

            if(insertedScore.Length > 0)
            {
                PlayerPrefs.SetString("last_score_id", insertedScore[0].id);
                PlayerPrefs.Save();
            }

            Debug.Log("Score enviado!");
        }
            
    }

    public IEnumerator GetTopScores()
    {
        string url =
            $"{supabaseUrl}/rest/v1/leaderboard?select=id,player_name,score,created_at&order=score.desc&order=created_at.desc&limit=10";

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
            ScoreEntry[] scores =
                JsonHelper.FromJson<ScoreEntry>(request.downloadHandler.text);

            ShowRanking(scores);
        }
    }

    void ShowRanking(ScoreEntry[] scores)
    {
        foreach(Transform child in rankingContent)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < scores.Length; i++)
        {
            GameObject row =
                Instantiate(
                    rankingRowPrefab,
                    rankingContent
                );

            RankingItemUI rankingRow =
                row.GetComponent<RankingItemUI>();

            rankingRow.Setup(
                i + 1,
                scores[i].player_name,
                scores[i].score
            );
        }
    }

    public IEnumerator GetPlayerRank(string playerName, int playerScore)
    {
        string url =
            $"{supabaseUrl}/rest/v1/leaderboard?select=player_name,score,created_at&order=score.desc&order=created_at.desc&limit=1000";

        UnityWebRequest request = UnityWebRequest.Get(url);

        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", "Bearer " + anonKey);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro posição: " + request.downloadHandler.text);
        }
        else
        {
            ScoreEntry[] scores =
                JsonHelper.FromJson<ScoreEntry>(request.downloadHandler.text);

            int rank = 1;

            for(int i = 0; i < scores.Length; i++)
            {
                if(scores[i].score > playerScore)
                {
                    rank++;
                }
                else if(scores[i].score == playerScore)
                {
                    break;
                }
            }

            myRankingText.text =
                $"Sua posição: #{rank}\n" +
                $"Jogador: {playerName}\n" +
                $"Score: {playerScore}";
        }
    }
    public IEnumerator DeleteOldScores(string playerName)
    {
        string url = $"{supabaseUrl}/rest/v1/leaderboard?player_name=eq.{UnityWebRequest.EscapeURL(playerName)}";

        UnityWebRequest request = UnityWebRequest.Delete(url);

        request.SetRequestHeader("apikey", anonKey);
        request.SetRequestHeader("Authorization", "Bearer " + anonKey);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao apagar score antigo: " + request.downloadHandler.text);
        }
    }
}
[System.Serializable]
public class ScoreEntry
{
    public string id;
    public string player_name;
    public int score;
    public string created_at;
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