/*
Responsabilidade:
Atualizar visualmente o timer da partida.

Controla:
- texto do tempo restante
- cor original do material TMP
- cor azul do freeze baseada no TNTSystem
- outline do freeze
*/

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [Header("References")]
    public TimerSystem timerSystem;
    public TextMeshProUGUI timerText;
    public TNTSystem tntSystem;

    [Header("FX")]
    public Outline freezeOutline;

    private int lastDisplayedSeconds = -1;

    private Material timerMaterialInstance;
    private Color originalTextColor;
    private Color originalFaceColor;
    private Color frozenColor;

    private void Awake()
    {
        SetupTimerMaterial();
    }

    private void Start()
    {
        SetFrozenColorFromTNTSystem();

        SetFrozenVisual(false);
        UpdateTimerText(true);
    }

    private void Update()
    {
        if (!CanUpdate())
            return;

        UpdateTimerText();
    }

    private bool CanUpdate()
    {
        return timerSystem != null &&
               timerText != null;
    }

    private void SetupTimerMaterial()
    {
        if (timerText == null)
            return;

        originalTextColor = timerText.color;

        timerMaterialInstance =
            new Material(timerText.fontSharedMaterial);

        timerText.fontMaterial = timerMaterialInstance;
        timerText.enableVertexGradient = false;

        originalFaceColor =
            timerMaterialInstance.GetColor(ShaderUtilities.ID_FaceColor);
    }

    private void SetFrozenColorFromTNTSystem()
    {
        if (tntSystem != null)
            frozenColor = tntSystem.buffEnergyColor;
        else
            frozenColor = new Color32(0, 255, 255, 255);
    }

    private void UpdateTimerText(bool forceUpdate = false)
    {
        int seconds =
            Mathf.CeilToInt(timerSystem.timeRemaining);

        if (!forceUpdate &&
            seconds == lastDisplayedSeconds)
            return;

        lastDisplayedSeconds = seconds;
        timerText.text = $"00:{seconds:00}";
    }

    public void SetFrozenVisual(bool isFrozen)
    {
        SetFrozenColorFromTNTSystem();

        if (isFrozen)
            ApplyFrozenColor();
        else
            RestoreOriginalColor();

        if (freezeOutline != null)
            freezeOutline.enabled = isFrozen;
    }

    private void ApplyFrozenColor()
    {
        if (timerText == null)
            return;

        timerText.color = frozenColor;

        if (timerMaterialInstance != null)
        {
            timerMaterialInstance.SetColor(
                ShaderUtilities.ID_FaceColor,
                frozenColor
            );
        }

        timerText.UpdateMeshPadding();
    }

    private void RestoreOriginalColor()
    {
        if (timerText == null)
            return;

        timerText.color = originalTextColor;

        if (timerMaterialInstance != null)
        {
            timerMaterialInstance.SetColor(
                ShaderUtilities.ID_FaceColor,
                originalFaceColor
            );
        }

        timerText.UpdateMeshPadding();
    }
}