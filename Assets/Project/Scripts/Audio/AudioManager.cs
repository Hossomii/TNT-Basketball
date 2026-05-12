/*
Responsabilidade:
Gerenciar áudio global do jogo.

Controla:
- efeitos sonoros
- música de fundo dos menus/telas
- música específica da gameplay

Regras:
- todas as cenas usam a música geral
- a cena Gameplay usa uma música própria

Dependências:
- InputHandler
- TNTSystem
- GameStartCountdown
- UI
- SceneManager
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Music")]
    public AudioClip generalBackgroundMusic;
    public AudioClip gameplayBackgroundMusic;

    [Header("Scene Names")]
    public string gameplaySceneName = "Gameplay";

    [Header("UI SFX")]
    public AudioClip click;
    public AudioClip switchSkin;

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

    [Header("Volume")]
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.45f;

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
        if (sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
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

    private void PlayMusic(AudioClip musicClip)
    {
        if (musicSource == null || musicClip == null)
            return;

        if (musicSource.clip == musicClip && musicSource.isPlaying)
            return;

        musicSource.clip = musicClip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayClick() => PlaySFX(click);
    public void PlaySwitchSkin() => PlaySFX(switchSkin);

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

    public void PlayCountdownGo() => PlaySFX(countdownGo);

    public void PlayShoot() => PlaySFX(shoot);
    public void PlayHit() => PlaySFX(hit);
    public void PlayMiss() => PlaySFX(miss);
    public void PlayPerfect() => PlaySFX(perfect);
    public void PlayCanActivate() => PlaySFX(canActivate);

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}