using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeedbackUI : MonoBehaviour
{
    public Image feedbackImage;

    public Sprite goodSprite;
    public Sprite perfectSprite;
    public Sprite missSprite;

    [Header("Animation")]
    public float fadeInTime = 0.15f;
    public float displayTime = 0.5f;
    public float fadeOutTime = 0.3f;
    public float scaleMultiplier = 1.2f;

    private Coroutine currentRoutine;

    private void Start()
    {
        feedbackImage.gameObject.SetActive(false);
    }

    public void Show(ShotEvaluator.ShotResult result)
    {
        if (feedbackImage == null) return;

        // Define sprite
        switch (result)
        {
            case ShotEvaluator.ShotResult.Good:
                feedbackImage.sprite = goodSprite;
                break;

            case ShotEvaluator.ShotResult.Perfect:
                feedbackImage.sprite = perfectSprite;
                break;

            case ShotEvaluator.ShotResult.Miss:
                feedbackImage.sprite = missSprite;
                break;
        }

        // Cancela animação anterior
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        feedbackImage.gameObject.SetActive(true);

        Color color = feedbackImage.color;
        color.a = 0f;
        feedbackImage.color = color;

        Vector3 startScale = Vector3.one * 0.7f;
        Vector3 targetScale = Vector3.one * scaleMultiplier;

        feedbackImage.transform.localScale = startScale;

        float time = 0f;

        // 🔹 Fade In + Scale Up
        while (time < fadeInTime)
        {
            time += Time.deltaTime;
            float t = time / fadeInTime;

            color.a = Mathf.Lerp(0f, 1f, t);
            feedbackImage.color = color;

            feedbackImage.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        feedbackImage.transform.localScale = Vector3.one;

        // 🔹 Tempo visível
        yield return new WaitForSeconds(displayTime);

        // 🔹 Fade Out
        time = 0f;
        while (time < fadeOutTime)
        {
            time += Time.deltaTime;
            float t = time / fadeOutTime;

            color.a = Mathf.Lerp(1f, 0f, t);
            feedbackImage.color = color;

            yield return null;
        }

        feedbackImage.gameObject.SetActive(false);
    }
}