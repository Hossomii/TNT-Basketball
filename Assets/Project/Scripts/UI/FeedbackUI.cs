/*
Responsabilidade:
Exibir o feedback visual do arremesso.

Mostra:
- Bom
- Perfeito
- Errou

Esse script cuida apenas da UI do feedback.
A pontuação, combo e animação da bola são controlados por outros sistemas.

Dependências:
- InputHandler: chama Show(result)
- ShotEvaluator: fornece o tipo de resultado
*/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackUI : MonoBehaviour
{
    [Header("UI")]
    public Image feedbackImage;

    [Header("Sprites")]
    public Sprite goodSprite;
    public Sprite perfectSprite;
    public Sprite missSprite;

    [Header("Base Animation")]
    public float fadeInTime = 0.12f;
    public float displayTime = 0.45f;
    public float fadeOutTime = 0.22f;

    [Header("Scale")]
    public float startScale = 0.7f;
    public float goodPopScale = 1.12f;
    public float perfectPopScale = 1.25f;
    public float missPopScale = 1.05f;

    [Header("Perfect Juice")]
    public bool usePerfectPulse = true;
    public float perfectPulseScale = 1.08f;
    public float perfectPulseTime = 0.08f;

    private Coroutine currentRoutine;

    private void Start()
    {
        HideFeedback();
    }

    /*
    Responsabilidade:
    Mostrar feedback de acordo com o resultado do arremesso.
    */
    public void Show(ShotEvaluator.ShotResult result)
    {
        if (feedbackImage == null)
            return;

        Sprite sprite = GetFeedbackSprite(result);

        if (sprite == null)
            return;

        feedbackImage.sprite = sprite;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PlayAnimation(result));
    }

    private Sprite GetFeedbackSprite(ShotEvaluator.ShotResult result)
    {
        switch (result)
        {
            case ShotEvaluator.ShotResult.Good:
                return goodSprite;

            case ShotEvaluator.ShotResult.Perfect:
                return perfectSprite;

            case ShotEvaluator.ShotResult.Miss:
                return missSprite;

            default:
                return missSprite;
        }
    }

    /*
    Responsabilidade:
    Animar entrada, permanência e saída do feedback.
    */
    private IEnumerator PlayAnimation(ShotEvaluator.ShotResult result)
    {
        feedbackImage.gameObject.SetActive(true);

        Color color = feedbackImage.color;
        color.a = 0f;
        feedbackImage.color = color;

        Vector3 initialScale = Vector3.one * startScale;
        Vector3 popScale = Vector3.one * GetPopScale(result);

        feedbackImage.transform.localScale = initialScale;

        yield return FadeAndScale(
            0f,
            1f,
            initialScale,
            popScale,
            fadeInTime
        );

        if (result == ShotEvaluator.ShotResult.Perfect && usePerfectPulse)
            yield return PlayPerfectPulse();

        feedbackImage.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(displayTime);

        yield return FadeAndScale(
            1f,
            0f,
            Vector3.one,
            Vector3.one * 0.95f,
            fadeOutTime
        );

        HideFeedback();
        currentRoutine = null;
    }

    /*
    Responsabilidade:
    Fazer o Perfect ter um micro pulso extra.
    */
    private IEnumerator PlayPerfectPulse()
    {
        Vector3 normalScale = Vector3.one;
        Vector3 pulseScale = Vector3.one * perfectPulseScale;

        float timer = 0f;

        while (timer < perfectPulseTime)
        {
            timer += Time.deltaTime;
            float t = timer / perfectPulseTime;

            feedbackImage.transform.localScale =
                Vector3.Lerp(pulseScale, normalScale, t);

            yield return null;
        }

        feedbackImage.transform.localScale = normalScale;
    }

    /*
    Responsabilidade:
    Animar alpha e scale ao mesmo tempo.
    */
    private IEnumerator FadeAndScale(
        float startAlpha,
        float endAlpha,
        Vector3 startScaleValue,
        Vector3 endScaleValue,
        float duration
    )
    {
        float timer = 0f;
        Color color = feedbackImage.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            feedbackImage.color = color;

            feedbackImage.transform.localScale =
                Vector3.Lerp(startScaleValue, endScaleValue, EaseOutBack(t));

            yield return null;
        }

        color.a = endAlpha;
        feedbackImage.color = color;
        feedbackImage.transform.localScale = endScaleValue;
    }

    private float GetPopScale(ShotEvaluator.ShotResult result)
    {
        switch (result)
        {
            case ShotEvaluator.ShotResult.Perfect:
                return perfectPopScale;

            case ShotEvaluator.ShotResult.Good:
                return goodPopScale;

            case ShotEvaluator.ShotResult.Miss:
                return missPopScale;

            default:
                return goodPopScale;
        }
    }

    /*
    Responsabilidade:
    Dar sensação de impacto arcade no pop.
    */
    private float EaseOutBack(float t)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    private void HideFeedback()
    {
        if (feedbackImage == null)
            return;

        feedbackImage.gameObject.SetActive(false);
    }
}