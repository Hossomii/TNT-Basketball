using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TNTSystem : MonoBehaviour
{
    [Header("Energy")]
    public int currentEnergy = 0;
    public int maxEnergy = 10;

    [Header("UI")]
    public TextMeshProUGUI energyText;
    public Image energyFill;

    [Header("Cans")]
    public TNTCanUI[] cans;
    public int currentCanIndex = 0;

    [Header("Power Ups")]
    public TNTPowerUpSystem powerUpSystem;

    public AudioManager audioManager;

    private void Start()
    {
        UpdateEnergyUI();
        UpdateCansUI();
    }

    public void AddEnergy(int amount)
    {
        currentEnergy += amount;

        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = 0;
            ActivateCurrentCan();
        }

        UpdateEnergyUI();
    }

    private void ActivateCurrentCan()
    {
        Debug.Log("TNT ATIVADO!");

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.canActivate);

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.canActivate);

        if (powerUpSystem != null)
            powerUpSystem.ActivatePowerUp(currentCanIndex);

        if (cans != null && currentCanIndex < cans.Length)
            cans[currentCanIndex].PlayActivation();

        currentCanIndex++;

        if (currentCanIndex >= cans.Length)
            currentCanIndex = 0;

        UpdateCansUI();
    }

    private void UpdateEnergyUI()
    {
        if (energyText != null)
            energyText.text = $"{currentEnergy}/{maxEnergy}";

        if (energyFill != null)
            energyFill.fillAmount = (float)currentEnergy / maxEnergy;
    }

    private void UpdateCansUI()
    {
        if (cans == null) return;

        for (int i = 0; i < cans.Length; i++)
        {
            cans[i].SetCurrent(i == currentCanIndex);
        }
    }
}