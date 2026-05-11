using UnityEngine;

public class BallVisualEffects : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private GameObject iceEffect;
    [SerializeField] private GameObject lightningEffect;

    private void Awake()
    {
        DisableAllEffects();
    }

    public void DisableAllEffects()
    {
        if (fireEffect != null)
            fireEffect.SetActive(false);

        if (iceEffect != null)
            iceEffect.SetActive(false);

        if (lightningEffect != null)
            lightningEffect.SetActive(false);
    }

    public void EnableFireEffect()
    {
        DisableAllEffects();

        if (fireEffect != null)
            fireEffect.SetActive(true);
    }

    public void EnableIceEffect()
    {
        DisableAllEffects();

        if (iceEffect != null)
            iceEffect.SetActive(true);
    }

    public void EnableLightningEffect()
    {
        DisableAllEffects();

        if (lightningEffect != null)
            lightningEffect.SetActive(true);
    }
}