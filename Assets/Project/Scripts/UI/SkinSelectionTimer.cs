using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SkinSelectionTimer : MonoBehaviour
{
    public BallSkinSelector skinSelector;
    public TextMeshProUGUI timerText;

    public float timeToSelect = 10f;
    public string gameplaySceneName = "Gameplay";

    private bool hasFinished = false;

    private void Update()
    {
        if (hasFinished) return;

        timeToSelect -= Time.deltaTime;

        int seconds = Mathf.CeilToInt(timeToSelect);

        if (timerText != null)
            timerText.text = seconds.ToString();

        if (timeToSelect <= 0f)
        {
            hasFinished = true;

            if (skinSelector != null)
                skinSelector.SaveCurrentSkin();

            SceneManager.LoadScene(gameplaySceneName);
        }
    }
}