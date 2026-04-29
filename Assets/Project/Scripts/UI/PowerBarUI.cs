using UnityEngine;

public class PowerBarUI : MonoBehaviour
{
    public PowerBar powerBar;
    public RectTransform aimPointer;

    [Header("Visual Offset")]
    public Vector2 pointerOffset = new Vector2(0f, -20f);

    private void Update()
    {
        if (powerBar == null || aimPointer == null)
            return;

        float value = powerBar.value;

        // Mantém a seta em um ponto fixo, sem esticar
        aimPointer.anchorMin = new Vector2(value, 0f);
        aimPointer.anchorMax = new Vector2(value, 0f);

        aimPointer.sizeDelta = new Vector2(24f, 32f);
        aimPointer.pivot = new Vector2(0.5f, 1f);
        aimPointer.anchoredPosition = pointerOffset;
    }
}