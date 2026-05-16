/*
Responsabilidade:
Gerenciar música, efeitos sonoros e sons de interface de forma global.

Usado por:
- Menu
- Gameplay
- Tutorial
- Ranking
- Skin Selector
- SettingsMenuController

Funções:
- Mantém áudio entre cenas
- Salva preferências do jogador
- Muta música e SFX separadamente
- Mantém compatibilidade com scripts antigos
*/

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;

    [Header("UI Clips")]
    [SerializeField] private AudioClip uiClickClip;
    [SerializeField] private AudioClip uiHoverClip;

    [Header("Gameplay Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip perfectClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip missClip;

    [Header("Countdown Clips")]
    [SerializeField] private AudioClip countdownNumberClip;
    [SerializeField] private AudioClip countdownGoClip;

    [Header("Game Flow Clips")]
    [SerializeField] private AudioClip finalWhistleClip;
    [SerializeField] private AudioClip crowdCheerClip;

    [Header("PowerUp Clips")]
    [SerializeField] private AudioClip canActivateClip;

    [Header("Skin Clips")]
    [SerializeField] private AudioClip switchSkinClip;
    [SerializeField] private AudioClip confirmSkinClip;

    private const string MUSIC_KEY = "music_enabled";
    private const string SFX_KEY = "sfx_enabled";

    private bool musicEnabled = true;
    private bool sfxEnabled = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAudioSettings();
        ApplyAudioSettings();
    }

    private void Start()
    {
        EnsureMusicIsPlaying();
    }

    private void EnsureMusicIsPlaying()
    {
        if (musicSource == null)
            return;

        if (musicSource.clip != null && !musicSource.isPlaying)
            musicSource.Play();

        ApplyAudioSettings();
    }

    public void SetMusicEnabled(bool enabled)
    {
        musicEnabled = enabled;

        PlayerPrefs.SetInt(MUSIC_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();

        if (musicEnabled)
            EnsureMusicIsPlaying();

        ApplyAudioSettings();
    }

    public void SetSfxEnabled(bool enabled)
    {
        sfxEnabled = enabled;

        PlayerPrefs.SetInt(SFX_KEY, enabled ? 1 : 0);
        PlayerPrefs.Save();

        ApplyAudioSettings();
    }

    public bool IsMusicEnabled()
    {
        return musicEnabled;
    }

    public bool IsSfxEnabled()
    {
        return sfxEnabled;
    }

    public void ApplyAudioSettings()
    {
        if (musicSource != null)
            musicSource.mute = !musicEnabled;

        if (sfxSource != null)
            sfxSource.mute = !sfxEnabled;

        if (uiSource != null)
            uiSource.mute = !sfxEnabled;
    }

    private void LoadAudioSettings()
    {
        musicEnabled = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt(SFX_KEY, 1) == 1;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null)
            return;

        if (musicSource.clip != clip)
            musicSource.clip = clip;

        if (!musicSource.isPlaying)
            musicSource.Play();

        ApplyAudioSettings();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (sfxSource == null || clip == null || !sfxEnabled)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayUI(AudioClip clip)
    {
        if (uiSource == null || clip == null || !sfxEnabled)
            return;

        uiSource.PlayOneShot(clip);
    }

    public void PlayUIClick() => PlayUI(uiClickClip);
    public void PlayUIHover() => PlayUI(uiHoverClip);

    public void PlayShoot() => PlaySfx(shootClip);
    public void PlayPerfect() => PlaySfx(perfectClip);
    public void PlayHit() => PlaySfx(hitClip);
    public void PlayMiss() => PlaySfx(missClip);

    public void PlayCountdownNumber() => PlaySfx(countdownNumberClip);
    public void PlayCountdownNumber(int number) => PlaySfx(countdownNumberClip);
    public void PlayCountdownNumber(string number) => PlaySfx(countdownNumberClip);

    public void PlayCountdownGo() => PlaySfx(countdownGoClip);

    public void PlayFinalWhistle() => PlaySfx(finalWhistleClip);
    public void PlayCrowdCheer() => PlaySfx(crowdCheerClip);

    public void PlayCanActivate() => PlaySfx(canActivateClip);

    public void PlaySwitchSkin() => PlaySfx(switchSkinClip);
    public void PlayConfirmSkin() => PlaySfx(confirmSkinClip);
}