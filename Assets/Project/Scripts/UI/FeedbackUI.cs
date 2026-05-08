/*
Responsabilidade:
Exibir feedback visual do resultado do arremesso.

Resultados:
- Good
- Perfect
- Miss

Usado por:
- InputHandler
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

    [Header("Animation")]
    public float fadeInTime = 0.15f;
    public float displayTime = 0.5f;
    public float fadeOutTime = 0.3f;
    public float startScale = 0.7f;
    public float popScale = 1.2f;

    private Coroutine currentRoutine;

    private void Start()
    {
        HideFeedback();
    }

    public void Show(ShotEvaluator.ShotResult result)
    {
        if (feedbackImage == null)
            return;

        feedbackImage.sprite = GetFeedbackSprite(result);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PlayAnimation());
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

    private IEnumerator PlayAnimation()
    {
        feedbackImage.gameObject.SetActive(true);

        Color color = feedbackImage.color;
        color.a = 0f;
        feedbackImage.color = color;

        Vector3 initialScale = Vector3.one * startScale;
        Vector3 targetScale = Vector3.one * popScale;

        feedbackImage.transform.localScale = initialScale;

        float timer = 0f;

        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            float t = timer / fadeInTime;

            color.a = Mathf.Lerp(0f, 1f, t);
            feedbackImage.color = color;

            feedbackImage.transform.localScale =
                Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        feedbackImage.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(displayTime);

        timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            float t = timer / fadeOutTime;

            color.a = Mathf.Lerp(1f, 0f, t);
            feedbackImage.color = color;

            yield return null;
        }

        HideFeedback();
    }

    private void HideFeedback()
    {
        if (feedbackImage == null)
            return;

        feedbackImage.gameObject.SetActive(false);
    }
}