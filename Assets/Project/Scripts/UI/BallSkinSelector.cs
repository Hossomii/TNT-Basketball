using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BallSkinSelector : MonoBehaviour
{
    [Header("Preview")]
    public Image ballPreview;

    [Header("Sprites")]
    public Sprite[] skins;

    [Header("Scene")]
    public string gameplaySceneName = "Gameplay";

    [Header("Input")]
    public float inputDelayOnStart = 0.25f;

    private int currentIndex = 0;
    private float inputDelayTimer;

    private void Start()
    {
        currentIndex = PlayerPrefs.GetInt("SelectedBallSkin", 0);
        inputDelayTimer = inputDelayOnStart;

        UpdateSkin();
    }

    private void Update()
    {
        if (inputDelayTimer > 0f)
        {
            inputDelayTimer -= Time.deltaTime;
            return;
        }

        if (Keyboard.current != null)
        {
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
                NextSkin();

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
                PreviousSkin();
        }
    }

    public void NextSkin()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.switchSkin);
        currentIndex++;

        if (currentIndex >= skins.Length)
            currentIndex = 0;

        UpdateSkin();
    }

    public void PreviousSkin()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.switchSkin);
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = skins.Length - 1;

        UpdateSkin();
    }

    public void SaveCurrentSkin()
    {
        PlayerPrefs.SetInt("SelectedBallSkin", currentIndex);
        PlayerPrefs.Save();
    }

    public void ConfirmSkin()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        SaveCurrentSkin();
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void UpdateSkin()
    {
        if (ballPreview != null && skins.Length > 0)
            ballPreview.sprite = skins[currentIndex];
    }
}