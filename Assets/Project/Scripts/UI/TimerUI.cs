/*
Responsabilidade:
Exibir o tempo restante da partida.

Formato:
00:60

Usado por:
- TimerSystem
- TNTPowerUpSystem
*/

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [Header("References")]
    public TimerSystem timerSystem;
    public TextMeshProUGUI timerText;

    [Header("Visual")]
    public Color normalColor = Color.yellow;
    public Color frozenColor = Color.cyan;

    [Header("FX")]
    public Outline freezeOutline;

    private void Start()
    {
        SetFrozenVisual(false);
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

    /*
    Responsabilidade:
    Atualizar texto do timer.
    */
    private void UpdateTimerText()
    {
        int seconds =
            Mathf.CeilToInt(timerSystem.timeRemaining);

        timerText.text =
            $"00:{seconds:00}";
    }

    /*
    Responsabilidade:
    Ativar visual de congelamento.
    */
    public void SetFrozenVisual(bool isFrozen)
    {
        if (timerText != null)
        {
            timerText.color =
                isFrozen
                ? frozenColor
                : normalColor;
        }

        if (freezeOutline != null)
        {
            freezeOutline.enabled = isFrozen;
        }
    }
}