/*
Responsabilidade:
Gerenciar efeitos sonoros do jogo.

Usado por:
- InputHandler
- TNTSystem
- BallSkinSelector
- UI
*/

using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("UI SFX")]
    public AudioClip click;
    public AudioClip switchSkin;

    [Header("Gameplay SFX")]
    public AudioClip shoot;
    public AudioClip hit;
    public AudioClip miss;
    public AudioClip perfect;
    public AudioClip canActivate;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private void Awake()
    {
        SetupSingleton();
        SetupAudioSource();
    }

    /*
    Responsabilidade:
    Garantir apenas um AudioManager.
    */
    private void SetupSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*
    Responsabilidade:
    Garantir AudioSource válido.
    */
    private void SetupAudioSource()
    {
        if (sfxSource == null)
        {
            sfxSource = GetComponent<AudioSource>();
        }
    }

    // =========================
    // UI
    // =========================

    public void PlayClick()
    {
        PlaySFX(click);
    }

    public void PlaySwitchSkin()
    {
        PlaySFX(switchSkin);
    }

    // =========================
    // Gameplay
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

    /*
    Responsabilidade:
    Tocar efeito sonoro.
    */
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        if (sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}