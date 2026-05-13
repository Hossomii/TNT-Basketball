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
        SetEffectActive(fireEffect, false);
        SetEffectActive(iceEffect, false);
        SetEffectActive(lightningEffect, false);
    }

    public void EnableFireEffect()
    {
        DisableAllEffects();
        SetEffectActive(fireEffect, true);
    }

    public void EnableIceEffect()
    {
        DisableAllEffects();
        SetEffectActive(iceEffect, true);
    }

    public void EnableLightningEffect()
    {
        DisableAllEffects();
        SetEffectActive(lightningEffect, true);
    }

    private void SetEffectActive(GameObject effect, bool active)
    {
        if (effect != null)
            effect.SetActive(active);
    }
}