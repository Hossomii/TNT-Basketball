/*
Responsabilidade:
Controlar o visual do multiplicador.

Mantém o multiplicador sempre visível e troca o sprite
quando o valor muda.

A animação é leve para não poluir a tela:
- pequena contração
- troca do sprite
- pequeno pop
- volta ao tamanho normal

Dependências:
- ScoreSystem: chama UpdateMultiplier()
- GameplayLockSystem: impede animação durante countdown
- TNTPowerUpSystem: chama SetDoubleScoreVisual(), mantido por compatibilidade
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierUI : MonoBehaviour
{
    [Header("UI")]
    public Image multiplierImage;

    [Header("Sprites")]
    public Sprite multiplier1x;
    public Sprite multiplier2x;
    public Sprite multiplier3x;
    public Sprite multiplier4x;
    public Sprite multiplier5x;
    public Sprite multiplier6x;

    [Header("Subtle Animation")]
    public float shrinkScale = 0.96f;
    public float popScale = 1.04f;
    public float animationTime = 0.10f;

    private Coroutine animationRoutine;
    private int currentMultiplierValue = -1;
    private Vector3 originalScale = Vector3.one;

    private void Awake()
    {
        if (multiplierImage != null)
            originalScale = multiplierImage.transform.localScale;
    }

    private void Start()
    {
        UpdateMultiplier(1f, true);
    }

    public void UpdateMultiplier(float multiplier)
    {
        UpdateMultiplier(multiplier, false);
    }

    private void UpdateMultiplier(float multiplier, bool forceInstant)
    {
        if (multiplierImage == null)
            return;

        multiplierImage.gameObject.SetActive(true);

        int newValue = GetMultiplierValue(multiplier);

        if (!forceInstant && newValue == currentMultiplierValue)
            return;

        Sprite newSprite = GetMultiplierSprite(newValue);

        if (forceInstant || GameplayLockSystem.IsGameplayLocked)
        {
            ApplyInstant(newValue, newSprite);
            return;
        }

        if (animationRoutine != null)
            StopCoroutine(animationRoutine);

        animationRoutine = StartCoroutine(
            PlaySubtleChangeAnimation(newValue, newSprite)
        );
    }

    private void ApplyInstant(int newValue, Sprite newSprite)
    {
        currentMultiplierValue = newValue;
        multiplierImage.sprite = newSprite;
        multiplierImage.transform.localScale = originalScale;
    }

    private IEnumerator PlaySubtleChangeAnimation(int newValue, Sprite newSprite)
    {
        currentMultiplierValue = newValue;

        Vector3 shrink = originalScale * shrinkScale;
        Vector3 pop = originalScale * popScale;

        yield return ScaleTo(shrink, animationTime * 0.35f);

        multiplierImage.sprite = newSprite;

        yield return ScaleTo(pop, animationTime * 0.35f);
        yield return ScaleTo(originalScale, animationTime * 0.30f);

        animationRoutine = null;
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = multiplierImage.transform.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            if (GameplayLockSystem.IsGameplayLocked)
            {
                multiplierImage.transform.localScale = originalScale;
                yield break;
            }

            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            multiplierImage.transform.localScale =
                Vector3.Lerp(startScale, targetScale, SmoothStep(t));

            yield return null;
        }

        multiplierImage.transform.localScale = targetScale;
    }

    private int GetMultiplierValue(float multiplier)
    {
        int value = Mathf.RoundToInt(multiplier);
        return Mathf.Clamp(value, 1, 6);
    }

    private Sprite GetMultiplierSprite(int value)
    {
        switch (value)
        {
            case 1:
                return multiplier1x;
            case 2:
                return multiplier2x;
            case 3:
                return multiplier3x;
            case 4:
                return multiplier4x;
            case 5:
                return multiplier5x;
            case 6:
                return multiplier6x;
            default:
                return multiplier1x;
        }
    }

    private float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }

    public void SetDoubleScoreVisual(bool active)
    {
        // Mantido por compatibilidade com TNTPowerUpSystem.
        // Os sprites já representam o multiplicador final.
    }
}