/*
Responsabilidade:
Controlar seleção de skins da bola.

Funcionalidades:
- Trocar skin
- Salvar skin selecionada
- Atualizar preview
- Confirmar seleção

Usado na cena:
- Skin Selection
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

    [Header("Scene")]
    public string gameplaySceneName = "Gameplay";

    [Header("Input")]
    public float inputDelayOnStart = 0.25f;

    [Header("PlayerPrefs")]
    public string saveKey = "SelectedBallSkin";

    private int currentIndex = 0;
    private float inputDelayTimer;

    private void Start()
    {
        LoadSavedSkin();

        inputDelayTimer = inputDelayOnStart;

        UpdateSkinPreview();
    }

    private void Update()
    {
        HandleInputDelay();

        if (inputDelayTimer > 0f)
            return;

        HandleKeyboardInput();
    }

    /*
    Responsabilidade:
    Impedir input instantâneo ao entrar na cena.
    */
    private void HandleInputDelay()
    {
        if (inputDelayTimer > 0f)
        {
            inputDelayTimer -= Time.deltaTime;
        }
    }

    /*
    Responsabilidade:
    Ler input do teclado.
    */
    private void HandleKeyboardInput()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            NextSkin();
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            PreviousSkin();
        }
    }

    public void NextSkin()
    {
        if (!HasValidSkins())
            return;

        PlaySwitchSound();

        currentIndex++;

        if (currentIndex >= skins.Length)
        {
            currentIndex = 0;
        }

        UpdateSkinPreview();
    }

    public void PreviousSkin()
    {
        if (!HasValidSkins())
            return;

        PlaySwitchSound();

        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = skins.Length - 1;
        }

        UpdateSkinPreview();
    }

    /*
    Responsabilidade:
    Confirmar skin e entrar na gameplay.
    */
    public void ConfirmSkin()
    {
        PlayClickSound();

        SaveCurrentSkin();

        SceneManager.LoadScene(gameplaySceneName);
    }

    /*
    Responsabilidade:
    Salvar skin selecionada.
    */
    public void SaveCurrentSkin()
    {
        PlayerPrefs.SetInt(saveKey, currentIndex);
        PlayerPrefs.Save();
    }

    /*
    Responsabilidade:
    Carregar skin salva anteriormente.
    */
    private void LoadSavedSkin()
    {
        currentIndex =
            PlayerPrefs.GetInt(saveKey, 0);

        currentIndex =
            Mathf.Clamp(currentIndex, 0, skins.Length - 1);
    }

    /*
    Responsabilidade:
    Atualizar preview visual da skin.
    */
    private void UpdateSkinPreview()
    {
        if (ballPreview == null)
            return;

        if (!HasValidSkins())
            return;

        ballPreview.sprite = skins[currentIndex];
    }

    private bool HasValidSkins()
    {
        return skins != null &&
               skins.Length > 0;
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