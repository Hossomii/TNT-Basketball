/*
Responsabilidade:
Controlar a posição e o tamanho das zonas Good e Perfect.

Esse script define onde ficam as zonas de acerto dentro da PowerBar.

Regras:
- no início da partida, as zonas ficam centralizadas
- depois de determinado tempo, as zonas randomizam após acertos
- o PowerUp TNT pode aumentar temporariamente a zona Perfect

Importante:
Esse script atualiza tanto:
- a lógica do arremesso no ShotEvaluator
- o visual das zonas no ZoneUI

Dependências:
- ShotEvaluator: recebe os valores reais das zonas
- ZoneUI: atualiza o tamanho/posição visual das zonas
- TimerSystem: define quando as zonas podem começar a randomizar
- TNTPowerUpSystem: ativa/desativa o boost da zona Perfect
*/

using UnityEngine;

public class ZoneRandomizer : MonoBehaviour
{
    [Header("References")]
    public ShotEvaluator shotEvaluator;
    public ZoneUI zoneUI;
    public TimerSystem timerSystem;

    [Header("Zone Sizes")]
    [Range(0f, 1f)] public float goodZoneSize = 0.5f;
    [Range(0f, 1f)] public float perfectZoneSize = 0.3f;

    [Header("Randomization")]
    public float randomizeAfterTime = 50f;

    [Header("TNT PowerUp")]
    public bool isPerfectBoostActive = false;
    [Range(0f, 1f)] public float boostedPerfectZoneSize = 0.18f;

    private const float CenterValue = 0.5f;

    private float lastCenter = CenterValue;

    private void Start()
    {
        SetCenteredZones();
    }

    /*
    Responsabilidade:
    Tentar randomizar as zonas.

    Antes do tempo definido:
    - mantém tudo centralizado

    Depois do tempo definido:
    - randomiza após acertos
    */
    public void TryRandomizeZones()
    {
        if (ShouldKeepCentered())
        {
            SetCenteredZones();
            return;
        }

        RandomizeZones();
    }

    /*
    Responsabilidade:
    Ativar ou desativar o boost da zona Perfect.

    Usado pela lata roxa.
    */
    public void SetPerfectZoneBoost(bool active, float boostedSize)
    {
        isPerfectBoostActive = active;
        boostedPerfectZoneSize = Mathf.Clamp01(boostedSize);

        ApplyZones(lastCenter);
    }

    /*
    Responsabilidade:
    Verificar se as zonas ainda devem ficar centralizadas.
    */
    private bool ShouldKeepCentered()
    {
        return timerSystem != null &&
               timerSystem.timeRemaining > randomizeAfterTime;
    }

    /*
    Responsabilidade:
    Centralizar as zonas.
    */
    private void SetCenteredZones()
    {
        lastCenter = CenterValue;
        ApplyZones(lastCenter);
    }

    /*
    Responsabilidade:
    Escolher uma posição aleatória segura para as zonas.

    A zona Good nunca deve sair para fora da barra.
    */
    private void RandomizeZones()
    {
        float safeGoodSize = Mathf.Clamp01(goodZoneSize);
        float safeHalfGood = safeGoodSize / 2f;

        lastCenter = Random.Range(
            safeHalfGood,
            1f - safeHalfGood
        );

        ApplyZones(lastCenter);
    }

    /*
    Responsabilidade:
    Aplicar os valores das zonas na lógica e na UI.
    */
    private void ApplyZones(float center)
    {
        float safeCenter = Mathf.Clamp01(center);
        float safeGoodSize = Mathf.Clamp01(goodZoneSize);
        float safePerfectSize = GetActivePerfectZoneSize(safeGoodSize);

        float halfGood = safeGoodSize / 2f;
        float halfPerfect = safePerfectSize / 2f;

        float goodStart = safeCenter - halfGood;
        float goodEnd = safeCenter + halfGood;

        float perfectStart = safeCenter - halfPerfect;
        float perfectEnd = safeCenter + halfPerfect;

        ApplyToShotEvaluator(
            goodStart,
            goodEnd,
            perfectStart,
            perfectEnd
        );

        ApplyToZoneUI(
            goodStart,
            goodEnd,
            perfectStart,
            perfectEnd
        );
    }

    /*
    Responsabilidade:
    Definir o tamanho atual da zona Perfect.

    Se o boost estiver ativo:
    - usa boostedPerfectZoneSize

    Caso contrário:
    - usa perfectZoneSize
    */
    private float GetActivePerfectZoneSize(float safeGoodSize)
    {
        float activePerfectSize = isPerfectBoostActive
            ? boostedPerfectZoneSize
            : perfectZoneSize;

        return Mathf.Clamp(
            activePerfectSize,
            0f,
            safeGoodSize
        );
    }

    private void ApplyToShotEvaluator(
        float goodStart,
        float goodEnd,
        float perfectStart,
        float perfectEnd
    )
    {
        if (shotEvaluator == null)
            return;

        shotEvaluator.SetZones(
            goodStart,
            goodEnd,
            perfectStart,
            perfectEnd
        );
    }

    private void ApplyToZoneUI(
        float goodStart,
        float goodEnd,
        float perfectStart,
        float perfectEnd
    )
    {
        if (zoneUI == null)
            return;

        zoneUI.UpdateZones(
            goodStart,
            goodEnd,
            perfectStart,
            perfectEnd
        );
    }
}