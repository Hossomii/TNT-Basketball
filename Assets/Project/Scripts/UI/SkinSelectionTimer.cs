/*
Responsabilidade:
Controlar o tempo limite da seleção de skin.

Quando o tempo acaba:
- confirma/salva a skin atual
- carrega a gameplay pelo BallSkinSelector

Importante:
Este script não altera a cor do texto.
A cor/gradiente deve ser configurada direto no TextMeshPro.
*/

using UnityEngine;
using TMPro;

public class SkinSelectionTimer : MonoBehaviour
{
    [Header("References")]
    public BallSkinSelector skinSelector;
    public TextMeshProUGUI timerText;

    [Header("Settings")]
    public float timeToSelect = 10f;

    private bool hasFinished = false;
    private int lastDisplayedSecond = -1;

    private void Update()
    {
        if (hasFinished)
            return;

        TickTimer();
    }

    private void TickTimer()
    {
        timeToSelect -= Time.deltaTime;

        int seconds = Mathf.CeilToInt(timeToSelect);
        seconds = Mathf.Max(seconds, 0);

        UpdateTimerText(seconds);

        if (timeToSelect <= 0f)
            FinishSelection();
    }

    private void UpdateTimerText(int seconds)
    {
        if (timerText == null)
            return;

        if (seconds == lastDisplayedSecond)
            return;

        lastDisplayedSecond = seconds;
        timerText.text = seconds.ToString();
    }

    private void FinishSelection()
    {
        hasFinished = true;

        if (skinSelector != null)
            skinSelector.ConfirmSkin();
    }
}