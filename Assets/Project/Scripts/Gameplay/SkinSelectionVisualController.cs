/*
Responsabilidade:
Controlar efeitos visuais da tela de seleção de skin.

Esse script cuida apenas do polimento visual:
- animação leve da bola ao trocar skin
- pulse da base
- feedback do botão confirmar
- transição curta antes da gameplay

Dependências:
- RectTransform da bola
- RectTransform da base
- CanvasGroup opcional para fade da tela
*/

using System;
using System.Collections;
using UnityEngine;

public class SkinSelectionVisualController : MonoBehaviour
{
    [Header("References")]
    public RectTransform ballPreview;
    public RectTransform ballBase;
    public RectTransform confirmButton;

    [Header("Fade")]
    public CanvasGroup fadeOverlayCanvasGroup;

    [Header("Skin Change Animation")]
    public float ballShrinkScale = 0.92f;
    public float ballPopScale = 1.06f;
    public float skinChangeDuration = 0.16f;

    [Header("Idle Animation")]
    public bool useIdleAnimation = true;
    public float idleAmplitude = 6f;
    public float idleSpeed = 2f;

    [Header("Confirm Transition")]
    public float confirmPopScale = 1.06f;
    public float fadeDuration = 0.25f;

    private Vector3 ballOriginalScale;
    private Vector3 ballOriginalPosition;
    private Vector3 baseOriginalScale;
    private Vector3 confirmOriginalScale;

    private Coroutine skinRoutine;
    private bool isTransitioning = false;

    private void Awake()
    {
        CacheOriginalValues();
    }

    private void Update()
    {
        if (useIdleAnimation && !isTransitioning)
            PlayIdleAnimation();
    }

    private void CacheOriginalValues()
    {
        if (ballPreview != null)
        {
            ballOriginalScale = ballPreview.localScale;
            ballOriginalPosition = ballPreview.localPosition;
        }

        if (ballBase != null)
            baseOriginalScale = ballBase.localScale;

        if (confirmButton != null)
            confirmOriginalScale = confirmButton.localScale;
    }

    public void PlaySkinChangeFeedback()
    {
        if (ballPreview == null)
            return;

        if (skinRoutine != null)
            StopCoroutine(skinRoutine);

        skinRoutine = StartCoroutine(SkinChangeRoutine());
    }

    private IEnumerator SkinChangeRoutine()
    {
        Vector3 shrink = ballOriginalScale * ballShrinkScale;
        Vector3 pop = ballOriginalScale * ballPopScale;

        yield return ScaleRect(ballPreview, shrink, skinChangeDuration * 0.35f);
        yield return ScaleRect(ballPreview, pop, skinChangeDuration * 0.35f);
        yield return ScaleRect(ballPreview, ballOriginalScale, skinChangeDuration * 0.30f);

        if (ballBase != null)
        {
            Vector3 basePop = baseOriginalScale * 1.03f;

            yield return ScaleRect(ballBase, basePop, 0.05f);
            yield return ScaleRect(ballBase, baseOriginalScale, 0.08f);
        }

        skinRoutine = null;
    }

    public void PlayConfirmTransition(Action onComplete)
    {
        if (!gameObject.activeInHierarchy)
        {
            onComplete?.Invoke();
            return;
        }

        StartCoroutine(ConfirmRoutine(onComplete));
    }

    private IEnumerator ConfirmRoutine(Action onComplete)
    {
        isTransitioning = true;

        if (confirmButton != null)
        {
            Vector3 pop = confirmOriginalScale * confirmPopScale;

            yield return ScaleRect(confirmButton, pop, 0.08f);
            yield return ScaleRect(confirmButton, confirmOriginalScale, 0.08f);
        }

        if (fadeOverlayCanvasGroup != null)
        {
            float timer = 0f;

            fadeOverlayCanvasGroup.alpha = 0f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;

                float t = Mathf.Clamp01(timer / fadeDuration);

                fadeOverlayCanvasGroup.alpha =
                    Mathf.Lerp(0f, 1f, t);

                yield return null;
            }

            fadeOverlayCanvasGroup.alpha = 1f;
        }

        onComplete?.Invoke();
    }

    private void PlayIdleAnimation()
    {
        if (ballPreview == null)
            return;

        float yOffset =
            Mathf.Sin(Time.time * idleSpeed) * idleAmplitude;

        ballPreview.localPosition =
            ballOriginalPosition + new Vector3(0f, yOffset, 0f);
    }

    private IEnumerator ScaleRect(
        RectTransform target,
        Vector3 targetScale,
        float duration
    )
    {
        if (target == null)
            yield break;

        Vector3 startScale = target.localScale;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            target.localScale =
                Vector3.Lerp(startScale, targetScale, SmoothStep(t));

            yield return null;
        }

        target.localScale = targetScale;
    }

    private float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }
}