/*
Responsabilidade:
Controlar todo o sistema de energia TNT e buffs temporários.

Esse sistema gerencia:
- energia TNT
- ativação automática das latas
- duração do buff atual
- preview visual da lata ativa
- ícone do poder ativo
- integração com a fila visual das latas

Fluxo:
1. jogador ganha energia ao acertar
2. ao atingir energia máxima:
   -> ativa a primeira lata da fila
3. buff é executado
4. UI mostra:
   - lata ativa
   - ícone do poder
   - tempo restante
5. quando o buff termina:
   -> UI é limpa
   -> próxima lata entra na fila

Dependências:
- TNTPowerUpSystem:
  executa os efeitos reais dos buffs

- TNTCanVisualController:
  controla a fila visual das latas

- InputHandler:
  chama AddEnergy() e ApplyMissPenalty()

- AudioManager:
  toca som de ativação da lata
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
        ResetVisuals();
        UpdateEnergyUI();
    }

    private void Update()
    {
        if (!CanUpdateBuff())
            return;

        UpdateBuffTimer();
    }

    /*
    Responsabilidade:
    Define se o buff atual precisa atualizar.
    */
    private bool CanUpdateBuff()
    {
        return isBuffActive;
    }

    /*
    Responsabilidade:
    Adicionar energia TNT ao acertar.
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
    Remover energia TNT ao errar.
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
    Ativar a lata atual da fila.
    */
    private void ActivateCurrentCan()
    {
        if (!CanActivateCan())
            return;

        activeCanIndex =
            visualController.GetCurrentCanIndex();

        activeBuffDuration =
            powerUpSystem.GetPowerUpDuration(activeCanIndex);

        activeBuffTimer =
            activeBuffDuration;

        isBuffActive = true;

        powerUpSystem.ActivatePowerUp(activeCanIndex);

        UpdateActiveCanUI(activeCanIndex);

        visualController.MoveCurrentCanToEnd();

        AudioManager.Instance?.PlayCanActivate();

        UpdateEnergyUI();

        if (enableLogs)
        {
            Debug.Log(
                $"TNTSystem | Activated Can: {activeCanIndex}"
            );
        }
    }

    /*
    Responsabilidade:
    Verificar se o sistema pode ativar uma lata.
    */
    private bool CanActivateCan()
    {
        return powerUpSystem != null &&
               visualController != null;
    }

    /*
    Responsabilidade:
    Atualizar o tempo restante do buff.
    */
    private void UpdateBuffTimer()
    {
        activeBuffTimer -= Time.deltaTime;

        if (activeBuffTimer <= 0f)
        {
            EndCurrentBuff();
            return;
        }

        UpdateEnergyUI();
    }

    /*
    Responsabilidade:
    Encerrar o buff atual.
    */
    private void EndCurrentBuff()
    {
        activeBuffTimer = 0f;

        isBuffActive = false;
        activeCanIndex = -1;

        ResetVisuals();

        UpdateEnergyUI();

        if (enableLogs)
        {
            Debug.Log("TNTSystem | Buff Ended");
        }
    }

    /*
    Responsabilidade:
    Atualizar visual da energia ou do buff.
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

    /*
    Responsabilidade:
    Atualizar UI da energia normal.
    */
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

    /*
    Responsabilidade:
    Atualizar UI durante buff ativo.
    */
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
    Atualizar preview e ícone da lata ativa.
    */
    private void UpdateActiveCanUI(int canIndex)
    {
        ShowActiveCanPreview(canIndex);
        ShowPowerIcon(canIndex);
    }

    /*
    Responsabilidade:
    Mostrar preview da lata ativa.
    */
    private void ShowActiveCanPreview(int canIndex)
    {
        if (activeCanPreviewImage == null)
            return;

        if (HasValidSprite(
            activeCanSprites,
            canIndex))
        {
            activeCanPreviewImage.sprite =
                activeCanSprites[canIndex];
        }

        activeCanPreviewImage.gameObject.SetActive(true);
    }

    /*
    Responsabilidade:
    Mostrar ícone do poder ativo.
    */
    private void ShowPowerIcon(int canIndex)
    {
        if (powerIconImage == null)
            return;

        if (HasValidSprite(
            powerIconSprites,
            canIndex))
        {
            powerIconImage.sprite =
                powerIconSprites[canIndex];
        }

        powerIconImage.gameObject.SetActive(true);
    }

    /*
    Responsabilidade:
    Resetar elementos visuais do buff.
    */
    private void ResetVisuals()
    {
        HideActiveCanPreview();
        HidePowerIcon();
    }

    private void HideActiveCanPreview()
    {
        if (activeCanPreviewImage != null)
        {
            activeCanPreviewImage.gameObject.SetActive(false);
        }
    }

    private void HidePowerIcon()
    {
        if (powerIconImage != null)
        {
            powerIconImage.gameObject.SetActive(false);
        }
    }

    /*
    Responsabilidade:
    Validar sprites antes de acessar arrays.
    */
    private bool HasValidSprite(
        Sprite[] sprites,
        int index)
    {
        return sprites != null &&
               index >= 0 &&
               index < sprites.Length;
    }
}