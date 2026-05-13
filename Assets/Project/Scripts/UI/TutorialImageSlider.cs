using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TutorialImageSlider : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image currentImage;
    [SerializeField] private Image nextImage;

    [Header("Slides")]
    [SerializeField] private Sprite[] tutorialSlides;

    [Header("Animation")]
    [SerializeField] private float transitionDuration = 0.35f;
    [SerializeField] private float slideDistance = 1920f;

    private int currentIndex = 0;
    private bool isTransitioning = false;

    private RectTransform currentRect;
    private RectTransform nextRect;
    private CanvasGroup currentGroup;
    private CanvasGroup nextGroup;

    private void Awake()
    {
        currentRect = currentImage.GetComponent<RectTransform>();
        nextRect = nextImage.GetComponent<RectTransform>();

        currentGroup = currentImage.GetComponent<CanvasGroup>();
        nextGroup = nextImage.GetComponent<CanvasGroup>();

        if (currentGroup == null)
            currentGroup = currentImage.gameObject.AddComponent<CanvasGroup>();

        if (nextGroup == null)
            nextGroup = nextImage.gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (tutorialSlides == null || tutorialSlides.Length == 0)
            return;

        currentIndex = 0;

        currentImage.sprite = tutorialSlides[currentIndex];
        currentImage.gameObject.SetActive(true);

        nextImage.gameObject.SetActive(false);

        currentRect.anchoredPosition = Vector2.zero;
        nextRect.anchoredPosition = Vector2.zero;

        currentGroup.alpha = 1f;
        nextGroup.alpha = 0f;
    }

    public void NextSlide()
    {
        if (isTransitioning || tutorialSlides.Length <= 1)
            return;

        int nextIndex = currentIndex + 1;

        if (nextIndex >= tutorialSlides.Length)
            nextIndex = 0;

        StartCoroutine(TransitionRoutine(nextIndex, 1));
    }

    public void PreviousSlide()
    {
        if (isTransitioning || tutorialSlides.Length <= 1)
            return;

        int nextIndex = currentIndex - 1;

        if (nextIndex < 0)
            nextIndex = tutorialSlides.Length - 1;

        StartCoroutine(TransitionRoutine(nextIndex, -1));
    }

    private IEnumerator TransitionRoutine(int nextIndex, int direction)
    {
        isTransitioning = true;

        nextImage.sprite = tutorialSlides[nextIndex];
        nextImage.gameObject.SetActive(true);

        currentRect.anchoredPosition = Vector2.zero;
        nextRect.anchoredPosition = new Vector2(slideDistance * direction, 0f);

        currentGroup.alpha = 1f;
        nextGroup.alpha = 0f;

        float elapsed = 0f;

        Vector2 currentStart = Vector2.zero;
        Vector2 currentEnd = new Vector2(-slideDistance * direction, 0f);

        Vector2 nextStart = new Vector2(slideDistance * direction, 0f);
        Vector2 nextEnd = Vector2.zero;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = elapsed / transitionDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            currentRect.anchoredPosition = Vector2.Lerp(currentStart, currentEnd, t);
            nextRect.anchoredPosition = Vector2.Lerp(nextStart, nextEnd, t);

            currentGroup.alpha = Mathf.Lerp(1f, 0f, t);
            nextGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        currentIndex = nextIndex;

        currentImage.sprite = tutorialSlides[currentIndex];

        currentRect.anchoredPosition = Vector2.zero;
        nextRect.anchoredPosition = Vector2.zero;

        currentGroup.alpha = 1f;
        nextGroup.alpha = 0f;

        nextImage.gameObject.SetActive(false);

        isTransitioning = false;
    }
}