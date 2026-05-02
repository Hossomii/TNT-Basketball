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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (sfxSource == null)
                sfxSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClick()
    {
        PlaySFX(click);
    }

    public void PlaySwitchSkin()
    {
        PlaySFX(switchSkin);
    }

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

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        if (sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip);
    }
}