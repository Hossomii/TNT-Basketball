using UnityEngine;

public class PowerBarUI : MonoBehaviour
{
    [Header("References")]
    public PowerBar powerBar;
    public RectTransform barRect;
    public RectTransform aimPointer;

    [Header("Visual")]
    public Vector2 pointerOffset = new Vector2(0f, 10f);

    private Vector2 originalSize;

    private void Start()
    {
        SetupPointer();
        UpdatePointerPosition();
    }

    private void Update()
    {
        if (!CanUpdate())
            return;

        UpdatePointerPosition();
    }

    private bool CanUpdate()
    {
        return powerBar != null &&
               barRect != null &&
               aimPointer != null;
    }

    private void SetupPointer()
    {
        if (aimPointer == null)
            return;

        originalSize = aimPointer.sizeDelta;

        aimPointer.anchorMin = new Vector2(0f, 0.5f);
        aimPointer.anchorMax = new Vector2(0f, 0.5f);
        aimPointer.pivot = new Vector2(0.5f, 0.5f);
        aimPointer.localScale = Vector3.one;
    }

    private void UpdatePointerPosition()
    {
        float value = Mathf.Clamp01(powerBar.value);

        float barWidth = barRect.rect.width;
        float xPosition = value * barWidth;

        aimPointer.anchoredPosition = new Vector2(
            xPosition + pointerOffset.x,
            pointerOffset.y
        );

        aimPointer.localScale = Vector3.one;
        aimPointer.sizeDelta = originalSize;
    }
}