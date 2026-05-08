/*
Responsabilidade:
Controlar:
- Energia TNT
- Buff ativo
- Preview da lata ativa
- Ícone do poder ativo
- Integração com a fila visual das latas

Fluxo:
- Acumula energia
- Quando chega no máximo, ativa a primeira lata da fila
- Executa o buff
- Mostra preview da lata ativa
- Mostra ícone do poder ativo
- Quando o buff termina, a lata usada vai para o final da fila
*/

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TNTSystem : MonoBehaviour
{
    [Header("Energy")]
    public int currentEnergy = 0;
    public int maxEnergy = 5;
    public int missEnergyPenalty = 2;

    [Header("Buff State")]
    public bool isBuffActive = false;

    public int activeCanIndex = -1;

    public float activeBuffDuration = 0f;
    public float activeBuffTimer = 0f;

    [Header("UI")]
    public TextMeshProUGUI energyText;
    public Image energyFill;

    [Header("UI Colors")]
    public Color normalEnergyColor = Color.cyan;
    public Color buffEnergyColor = Color.yellow;

    [Header("Preview")]
    public Image activeCanPreviewImage;
    public Sprite[] activeCanSprites;

    [Header("Power Icon")]
    public Image powerIconImage;
    public Sprite[] powerIconSprites;

    [Header("Systems")]
    public TNTPowerUpSystem powerUpSystem;
    public TNTCanVisualController visualController;

    [Header("Debug")]
    public bool enableLogs = false;

    private void Start()
    {
        HideActiveCanPreview();
        HidePowerIcon();

        UpdateEnergyUI();
    }

    private void Update()
    {
        if (!isBuffActive)
            return;

        UpdateBuffTimer();
    }

    /*
    Responsabilidade:
    Adicionar energia TNT.
    */
    public void AddEnergy(int amount)
    {
        if (isBuffActive)
            return;

        currentEnergy += amount;

        currentEnergy = Mathf.Clamp(
            currentEnergy,
            0,
            maxEnergy
        );

        if (currentEnergy >= maxEnergy)
        {
            currentEnergy = 0;
            ActivateCurrentCan();
        }

        UpdateEnergyUI();
    }

    /*
    Responsabilidade:
    Penalizar energia ao errar.
    */
    public void ApplyMissPenalty()
    {
        if (isBuffActive)
            return;

        currentEnergy -= missEnergyPenalty;

        currentEnergy = Mathf.Clamp(
            currentEnergy,
            0,
            maxEnergy
        );

        UpdateEnergyUI();
    }

    /*
    Responsabilidade:
    Ativar a primeira lata da fila.
    */
    private void ActivateCurrentCan()
    {
        if (powerUpSystem == null || visualController == null)
            return;

        activeCanIndex = visualController.GetCurrentCanIndex();

        activeBuffDuration =
            powerUpSystem.GetPowerUpDuration(activeCanIndex);

        activeBuffTimer =
            activeBuffDuration;

        isBuffActive = true;

        powerUpSystem.ActivatePowerUp(activeCanIndex);

        ShowActiveCanPreview(activeCanIndex);
        ShowPowerIcon(activeCanIndex);

        AudioManager.Instance?.PlayCanActivate();

        UpdateEnergyUI();

        if (enableLogs)
        {
            Debug.Log($"Activated Can: {activeCanIndex}");
        }
    }

    /*
    Responsabilidade:
    Atualizar contagem do buff ativo.
    */
    private void UpdateBuffTimer()
    {
        activeBuffTimer -= Time.deltaTime;

        if (activeBuffTimer <= 0f)
        {
            EndCurrentBuff();
        }

        UpdateEnergyUI();
    }

    /*
    Responsabilidade:
    Encerrar buff atual e mandar a lata usada para o final da fila.
    */
    private void EndCurrentBuff()
    {
        activeBuffTimer = 0f;

        isBuffActive = false;
        activeCanIndex = -1;

        HideActiveCanPreview();
        HidePowerIcon();

        if (visualController != null)
            visualController.MoveCurrentCanToEnd();

        UpdateEnergyUI();

        if (enableLogs)
        {
            Debug.Log("Buff Ended");
        }
    }

    /*
    Responsabilidade:
    Atualizar UI da energia ou do tempo restante do buff.
    */
    private void UpdateEnergyUI()
    {
        if (isBuffActive)
        {
            UpdateBuffUI();
            return;
        }

        UpdateNormalEnergyUI();
    }

    private void UpdateNormalEnergyUI()
    {
        if (energyText != null)
        {
            energyText.text =
                $"{currentEnergy}/{maxEnergy}";
        }

        if (energyFill != null)
        {
            energyFill.fillAmount =
                (float)currentEnergy / maxEnergy;

            energyFill.color =
                normalEnergyColor;
        }
    }

    private void UpdateBuffUI()
    {
        if (energyText != null)
        {
            energyText.text =
                $"{Mathf.CeilToInt(activeBuffTimer)}s";
        }

        if (energyFill != null)
        {
            energyFill.fillAmount =
                activeBuffDuration > 0f
                ? activeBuffTimer / activeBuffDuration
                : 0f;

            energyFill.color =
                buffEnergyColor;
        }
    }

    /*
    Responsabilidade:
    Mostrar preview da lata ativa.
    */
    private void ShowActiveCanPreview(int canIndex)
    {
        if (activeCanPreviewImage == null)
            return;

        if (activeCanSprites != null &&
            canIndex >= 0 &&
            canIndex < activeCanSprites.Length)
        {
            activeCanPreviewImage.sprite =
                activeCanSprites[canIndex];
        }

        activeCanPreviewImage.gameObject.SetActive(true);
    }

    private void HideActiveCanPreview()
    {
        if (activeCanPreviewImage != null)
        {
            activeCanPreviewImage.gameObject.SetActive(false);
        }
    }

    /*
    Responsabilidade:
    Mostrar ícone do poder ativo.
    */
    private void ShowPowerIcon(int canIndex)
    {
        if (powerIconImage == null)
            return;

        if (powerIconSprites != null &&
            canIndex >= 0 &&
            canIndex < powerIconSprites.Length)
        {
            powerIconImage.sprite =
                powerIconSprites[canIndex];
        }

        powerIconImage.gameObject.SetActive(true);
    }

    private void HidePowerIcon()
    {
        if (powerIconImage != null)
        {
            powerIconImage.gameObject.SetActive(false);
        }
    }
}