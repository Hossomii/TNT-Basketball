using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TNTCanUI : MonoBehaviour
{
    [Header("UI")]
    public Image canImage;

    [Header("Colors")]
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.35f);

    [Header("Animation")]
    public float bounceScale = 1.2f;
    public float bounceTime = 0.1f;

    [Header("Glow")]
    public Image glowImage;
    public float glowMaxAlpha = 0.6f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        SetGlow(0f);
    }

    public void SetCurrent(bool isCurrent)
    {
        if (canImage != null)
            canImage.color = isCurrent ? activeColor : inactiveColor;

        SetGlow(isCurrent ? glowMaxAlpha : 0f);
    }

    public void PlayActivation()
    {
        StopAllCoroutines();
        StartCoroutine(ActivationRoutine());
    }

    private IEnumerator ActivationRoutine()
    {
        transform.localScale = originalScale * bounceScale;

        yield return new WaitForSeconds(bounceTime);

        transform.localScale = originalScale;
    }

    private void SetGlow(float alpha)
    {
        if (glowImage == null) return;

        Color c = glowImage.color;
        c.a = alpha;
        glowImage.color = c;
    }
}