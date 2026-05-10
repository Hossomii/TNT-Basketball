/*
Responsabilidade:
Controlar visual do multiplicador.

Como funciona:
- Troca sprite baseado no multiplicador atual.
- Usa PNGs prontos para 1x até 6x.

Usado por:
- ScoreSystem
*/

using UnityEngine;
using UnityEngine.UI;

public class MultiplierUI : MonoBehaviour
{
    [Header("UI")]
    public Image multiplierImage;

    [Header("Sprites")]
    public Sprite multiplier1x;
    public Sprite multiplier2x;
    public Sprite multiplier3x;
    public Sprite multiplier4x;
    public Sprite multiplier5x;
    public Sprite multiplier6x;

    private void Start()
    {
        UpdateMultiplier(1f);
    }

    public void UpdateMultiplier(float multiplier)
    {
        if (multiplierImage == null)
            return;

        multiplierImage.gameObject.SetActive(true);
        multiplierImage.sprite = GetMultiplierSprite(multiplier);
    }

    public void SetDoubleScoreVisual(bool active)
    {
        // Mantido vazio de propósito.
        // Os sprites PNG já carregam o visual do multiplicador.
        // Esse método continua existindo para não quebrar chamadas do TNTPowerUpSystem.
    }

    private Sprite GetMultiplierSprite(float multiplier)
    {
        int value = Mathf.RoundToInt(multiplier);
        value = Mathf.Clamp(value, 1, 6);

        switch (value)
        {
            case 1:
                return multiplier1x;

            case 2:
                return multiplier2x;

            case 3:
                return multiplier3x;

            case 4:
                return multiplier4x;

            case 5:
                return multiplier5x;

            case 6:
                return multiplier6x;

            default:
                return multiplier1x;
        }
    }
}