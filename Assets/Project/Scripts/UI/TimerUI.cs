/*
Responsabilidade:
Exibir o tempo restante da partida na tela.

Formato:
TEMPO
00:60
*/

using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TimerSystem timerSystem;
    public TextMeshProUGUI timerText;

    private void Update()
    {
        if (timerSystem == null || timerText == null) return;

        int seconds = Mathf.CeilToInt(timerSystem.timeRemaining);

        timerText.text = $"TEMPO\n00:{seconds:00}";
    }
}