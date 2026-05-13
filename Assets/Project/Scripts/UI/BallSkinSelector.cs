/*
Responsabilidade:
Controlar a lógica de seleção de skins da bola.

Esse script cuida de:
- trocar skin
- salvar skin selecionada
- atualizar preview
- confirmar seleção
- carregar a gameplay

Dependências:
- Image da bola preview
- SkinSelectionVisualController para efeitos visuais
- AudioManager para sons
- PlayerPrefs para salvar a skin
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BallSkinSelector : MonoBehaviour
{
    [Header("Preview")]
    public Image ballPreview;

    [Header("Skins")]
    public Sprite[] skins;

    [Header("Visual")]
    public SkinSelectionVisualController visualController;

    [Header("Scene")]
    public string gameplaySceneName = "Gameplay";

    [Header("Input")]
    public float inputDelayOnStart = 0.25f;

    [Header("PlayerPrefs")]
    public string saveKey = "SelectedBallSkin";

    private int currentIndex = 0;
    private float inputDelayTimer;
    private bool isConfirming = false;

    private void Start()
    {
        LoadSavedSkin();

        inputDelayTimer = inputDelayOnStart;

        UpdateSkinPreview(false);
    }

    private void Update()
    {
        HandleInputDelay();

        if (inputDelayTimer > 0f || isConfirming)
            return;

        HandleKeyboardInput();
    }

    private void HandleInputDelay()
    {
        if (inputDelayTimer > 0f)
            inputDelayTimer -= Time.deltaTime;
    }

    private void HandleKeyboardInput()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            NextSkin();

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            PreviousSkin();
    }

    public void NextSkin()
    {
        if (!CanChangeSkin())
            return;

        PlaySwitchSound();

        currentIndex++;

        if (currentIndex >= skins.Length)
            currentIndex = 0;

        UpdateSkinPreview(true);
    }

    public void PreviousSkin()
    {
        if (!CanChangeSkin())
            return;

        PlaySwitchSound();

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = skins.Length - 1;

        UpdateSkinPreview(true);
    }

    public void ConfirmSkin()
    {
        if (isConfirming)
            return;

        isConfirming = true;

        Debug.Log("ConfirmSkin chamado");

        PlayClickSound();
        SaveCurrentSkin();

        if (visualController != null)
        {
            Debug.Log("Fade/transição visual iniciado");
            visualController.PlayConfirmTransition(LoadGameplay);
        }
        else
        {
            Debug.LogWarning("VisualController está vazio. Indo direto para gameplay.");
            LoadGameplay();
        }
    }

    private void LoadGameplay()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void SaveCurrentSkin()
    {
        PlayerPrefs.SetInt(saveKey, currentIndex);
        PlayerPrefs.Save();
    }

    private void LoadSavedSkin()
    {
        if (!HasValidSkins())
        {
            currentIndex = 0;
            return;
        }

        currentIndex = PlayerPrefs.GetInt(saveKey, 0);
        currentIndex = Mathf.Clamp(currentIndex, 0, skins.Length - 1);
    }

    private void UpdateSkinPreview(bool animate)
    {
        if (ballPreview == null || !HasValidSkins())
            return;

        ballPreview.sprite = skins[currentIndex];

        if (animate && visualController != null)
            visualController.PlaySkinChangeFeedback();
    }

    private bool CanChangeSkin()
    {
        return HasValidSkins() && !isConfirming;
    }

    private bool HasValidSkins()
    {
        return skins != null && skins.Length > 0;
    }

    private void PlaySwitchSound()
    {
        AudioManager.Instance?.PlaySwitchSkin();
    }

    private void PlayClickSound()
    {
        AudioManager.Instance?.PlayClick();
    }
}