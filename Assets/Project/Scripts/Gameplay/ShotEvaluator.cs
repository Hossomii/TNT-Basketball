/*
Responsabilidade:
Avaliar o resultado do arremesso usando a posição atual do ponteiro da barra.

Esse script NÃO move a barra e NÃO altera a UI.
Ele apenas lê o valor atual da PowerBar e compara com as zonas configuradas.

Resultados possíveis:
- Miss: fora das zonas de acerto
- Good: dentro da zona boa
- Perfect: dentro da zona perfeita

Dependências:
- PowerBar: fornece o valor atual do ponteiro entre 0 e 1
- ZoneRandomizer: pode atualizar as zonas dinamicamente usando SetZones()
- InputHandler: chama Evaluate() quando o jogador arremessa

Fluxo:
InputHandler
-> ShotEvaluator.Evaluate()
-> retorna Miss, Good ou Perfect
*/

using UnityEngine;

public class ShotEvaluator : MonoBehaviour
{
    [Header("References")]
    public PowerBar powerBar;

    [Header("Good Zone")]
    [Range(0f, 1f)] public float goodStart = 0.35f;
    [Range(0f, 1f)] public float goodEnd = 0.65f;

    [Header("Perfect Zone")]
    [Range(0f, 1f)] public float perfectStart = 0.45f;
    [Range(0f, 1f)] public float perfectEnd = 0.55f;

    [Header("Precision")]
    public float tolerance = 0.005f;

    [Header("Debug")]
    public bool enableLogs = false;

    public enum ShotResult
    {
        Miss,
        Good,
        Perfect
    }

    /*
    Responsabilidade:
    Avaliar o arremesso atual.
    */
    public ShotResult Evaluate()
    {
        if (powerBar == null)
        {
            Debug.LogWarning("ShotEvaluator: PowerBar não configurada.");
            return ShotResult.Miss;
        }

        float value = Mathf.Clamp01(powerBar.value);

        if (enableLogs)
            LogEvaluation(value);

        if (IsInsidePerfectZone(value))
            return ShotResult.Perfect;

        if (IsInsideGoodZone(value))
            return ShotResult.Good;

        return ShotResult.Miss;
    }

    /*
    Responsabilidade:
    Verificar se o valor está dentro da zona Perfect.

    Perfect é verificado antes do Good porque normalmente
    a zona Perfect fica dentro da zona Good.
    */
    private bool IsInsidePerfectZone(float value)
    {
        return IsInsideZone(
            value,
            perfectStart,
            perfectEnd
        );
    }

    /*
    Responsabilidade:
    Verificar se o valor está dentro da zona Good.
    */
    private bool IsInsideGoodZone(float value)
    {
        return IsInsideZone(
            value,
            goodStart,
            goodEnd
        );
    }

    /*
    Responsabilidade:
    Comparar um valor com uma zona usando tolerância.

    A tolerância ajuda a deixar o jogo menos injusto
    em cliques muito próximos da borda.
    */
    private bool IsInsideZone(
        float value,
        float start,
        float end
    )
    {
        return value >= start - tolerance &&
               value <= end + tolerance;
    }

    /*
    Responsabilidade:
    Atualizar as zonas dinamicamente.

    Usado pelo ZoneRandomizer para mover as áreas de acerto
    durante a partida.
    */
    public void SetZones(
        float newGoodStart,
        float newGoodEnd,
        float newPerfectStart,
        float newPerfectEnd
    )
    {
        goodStart = Mathf.Clamp01(newGoodStart);
        goodEnd = Mathf.Clamp01(newGoodEnd);

        perfectStart = Mathf.Clamp01(newPerfectStart);
        perfectEnd = Mathf.Clamp01(newPerfectEnd);

        ValidateZones();
    }

    /*
    Responsabilidade:
    Evitar zonas invertidas.

    Exemplo de problema:
    start = 0.70
    end = 0.30

    Isso quebraria a leitura da zona.
    */
    private void ValidateZones()
    {
        if (goodStart > goodEnd)
            Swap(ref goodStart, ref goodEnd);

        if (perfectStart > perfectEnd)
            Swap(ref perfectStart, ref perfectEnd);
    }

    private void Swap(ref float a, ref float b)
    {
        float temp = a;
        a = b;
        b = temp;
    }

    private void LogEvaluation(float value)
    {
        Debug.Log(
            $"ShotEvaluator | Value: {value:F3} | " +
            $"Perfect: {perfectStart:F3}-{perfectEnd:F3} | " +
            $"Good: {goodStart:F3}-{goodEnd:F3}"
        );
    }
}