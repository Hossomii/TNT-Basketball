/*
Responsabilidade:
Controlar o menu de configurações.

Funções:
- Abrir e fechar painel
- Atualizar toggles de música e SFX
- Pausar gameplay quando necessário
- Abrir/fechar com botão ou ESC
- Conversar apenas com o AudioManager global
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SettingsMenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button closeButton;

    [Header("Audio Toggles")]
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private Toggle sfxToggle;

    [Header("Pause")]
    [SerializeField] private bool pauseGameplay = false;

    private bool isOpen = false;
    private float previousTimeScale = 1f;

    private void Awake()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    private void Start()
    {
        settingsButton?.onClick.RemoveListener(TogglePanel);
        closeButton?.onClick.RemoveListener(ClosePanel);

        settingsButton?.onClick.AddListener(TogglePanel);
        closeButton?.onClick.AddListener(ClosePanel);

        SyncTogglesWithAudioManager();

        musicToggle?.onValueChanged.RemoveListener(OnMusicToggleChanged);
        sfxToggle?.onValueChanged.RemoveListener(OnSfxToggleChanged);

        musicToggle?.onValueChanged.AddListener(OnMusicToggleChanged);
        sfxToggle?.onValueChanged.AddListener(OnSfxToggleChanged);
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        if (isOpen)
            ClosePanel();
        else
            OpenPanel();
    }

    public void OpenPanel()
    {
        if (settingsPanel == null)
            return;

        settingsPanel.SetActive(true);
        isOpen = true;

        if (pauseGameplay)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        SyncTogglesWithAudioManager();
    }

    public void ClosePanel()
    {
        if (settingsPanel == null)
            return;

        settingsPanel.SetActive(false);
        isOpen = false;

        if (pauseGameplay)
            Time.timeScale = previousTimeScale <= 0f ? 1f : previousTimeScale;
    }

    private void SyncTogglesWithAudioManager()
    {
        if (AudioManager.Instance == null)
            return;

        musicToggle?.SetIsOnWithoutNotify(AudioManager.Instance.IsMusicEnabled());
        sfxToggle?.SetIsOnWithoutNotify(AudioManager.Instance.IsSfxEnabled());

        AudioManager.Instance.ApplyAudioSettings();
    }

    private void OnMusicToggleChanged(bool enabled)
    {
        AudioManager.Instance?.SetMusicEnabled(enabled);
    }

    private void OnSfxToggleChanged(bool enabled)
    {
        AudioManager.Instance?.SetSfxEnabled(enabled);
    }

    private void OnDestroy()
    {
        if (pauseGameplay && isOpen)
            Time.timeScale = 1f;
    }
}