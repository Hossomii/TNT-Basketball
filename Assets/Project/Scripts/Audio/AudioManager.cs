/*
Responsabilidade:
Gerenciar músicas e efeitos sonoros do jogo.

Controla:
- música geral dos menus
- música da gameplay
- efeitos de UI
- efeitos da gameplay
- sons do countdown
- apito final
- torcida final

Usado por:
- MenuUI
- BallSkinSelector
- InputHandler
- TNTSystem
- GameStartCountdown
- GameManager
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    [Header("Music")]
    public AudioClip generalBackgroundMusic;
    public AudioClip gameplayBackgroundMusic;
    public string gameplaySceneName = "Gameplay";

    [Header("UI SFX")]
    public AudioClip uiClick;
    public AudioClip uiHover;
    public AudioClip switchSkin;
    public AudioClip confirmSkin;

    [Header("Countdown SFX")]
    public AudioClip countdown3;
    public AudioClip countdown2;
    public AudioClip countdown1;
    public AudioClip countdownGo;

    [Header("Gameplay SFX")]
    public AudioClip shoot;
    public AudioClip hit;
    public AudioClip miss;
    public AudioClip perfect;
    public AudioClip canActivate;

    [Header("End Game SFX")]
    public AudioClip finalWhistle;
    public AudioClip crowdCheer;

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.45f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float uiVolume = 1f;
    [Range(0f, 1f)] public float finalWhistleVolume = 1f;
    [Range(0f, 1f)] public float crowdCheerVolume = 0.75f;

    private void Awake()
    {
        SetupSingleton();
        SetupAudioSources();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void SetupAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        if (uiSource == null)
        {
            uiSource = gameObject.AddComponent<AudioSource>();
            uiSource.loop = false;
            uiSource.playOnAwake = false;
        }

        musicSource.volume = musicVolume;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip targetMusic =
            sceneName == gameplaySceneName
                ? gameplayBackgroundMusic
                : generalBackgroundMusic;

        PlayMusic(targetMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);

        if (musicSource != null)
            musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public void SetUIVolume(float volume)
    {
        uiVolume = Mathf.Clamp01(volume);
    }

    public void SetFinalWhistleVolume(float volume)
    {
        finalWhistleVolume = Mathf.Clamp01(volume);
    }

    public void SetCrowdCheerVolume(float volume)
    {
        crowdCheerVolume = Mathf.Clamp01(volume);
    }

    public void PlaySFX(AudioClip clip)
    {
        PlaySFX(clip, sfxVolume);
    }

    private void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    private void PlayUISFX(AudioClip clip)
    {
        if (uiSource == null)
        {
            Debug.LogWarning("UI Source vazio");
            return;
        }

        if (clip == null)
        {
            Debug.LogWarning("UI Click Clip vazio");
            return;
        }

        Debug.Log("Tocando som de UI: " + clip.name);

        uiSource.PlayOneShot(clip, uiVolume);
    }

    // =========================
    // UI
    // =========================

    public void PlayUIClick()
    {
        Debug.Log("PlayUIClick chamado");

        PlayUISFX(uiClick);
    }

    public void PlayUIHover()
    {
        PlayUISFX(uiHover);
    }

    // compatibilidade
    public void PlayClick()
    {
        
        PlayUIClick();
    }

    public void PlaySwitchSkin()
    {
        PlayUISFX(switchSkin);
    }

    public void PlayConfirmSkin()
    {
        PlayUISFX(confirmSkin);
    }

    // =========================
    // COUNTDOWN
    // =========================

    public void PlayCountdownNumber(int number)
    {
        switch (number)
        {
            case 3:
                PlaySFX(countdown3);
                break;

            case 2:
                PlaySFX(countdown2);
                break;

            case 1:
                PlaySFX(countdown1);
                break;
        }
    }

    public void PlayCountdownGo()
    {
        PlaySFX(countdownGo);
    }

    // =========================
    // GAMEPLAY
    // =========================

    public void PlayShoot()
    {
        PlaySFX(shoot);
    }

    public void PlayHit()
    {
        PlaySFX(hit);
    }

    public void PlayMiss()
    {
        PlaySFX(miss);
    }

    public void PlayPerfect()
    {
        PlaySFX(perfect);
    }

    public void PlayCanActivate()
    {
        PlaySFX(canActivate);
    }

    // =========================
    // END GAME
    // =========================

    public void PlayFinalWhistle()
    {
        PlaySFX(finalWhistle, finalWhistleVolume);
    }

    public void PlayCrowdCheer()
    {
        PlaySFX(crowdCheer, crowdCheerVolume);
    }
}