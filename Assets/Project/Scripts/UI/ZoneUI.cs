/*
Responsabilidade:
Atualizar visualmente as zonas Good e Perfect dentro da PowerBar.

Esse script altera APENAS a UI.
Ele não calcula resultado de arremesso e não altera gameplay.

Dependências:
- ZoneRandomizer: envia os valores normalizados das zonas
- RectTransform goodZone: visual da zona Good
- RectTransform perfectZone: visual da zona Perfect
- Outline: feedback visual quando o buff da lata roxa está ativo

Fluxo:
ZoneRandomizer
-> ZoneUI.UpdateZones()
-> atualiza anchors das zonas na barra
*/

using UnityEngine;
using UnityEngine.UI;

public class ZoneUI : MonoBehaviour
{
    [Header("References")]
    public RectTransform goodZone;
    public RectTransform perfectZone;

    [Header("FX")]
    public Outline perfectZoneOutline;

    /*
    Responsabilidade:
    Atualizar as zonas visuais da UI.
    */
    public void UpdateZones(
        float goodStart,
        float goodEnd,
        float perfectStart,
        float perfectEnd
    )
    {
        UpdateZone(goodZone, goodStart, goodEnd);
        UpdateZone(perfectZone, perfectStart, perfectEnd);
    }

    /*
    Responsabilidade:
    Atualizar anchors de uma zona específica.

    Os valores são sempre protegidos entre 0 e 1
    para evitar que a zona saia da barra.
    */
    private void UpdateZone(
        RectTransform zone,
        float start,
        float end
    )
    {
        if (zone == null)
            return;

        float safeStart = Mathf.Clamp01(start);
        float safeEnd = Mathf.Clamp01(end);

        if (safeStart > safeEnd)
        {
            float temp = safeStart;
            safeStart = safeEnd;
            safeEnd = temp;
        }

        zone.anchorMin = new Vector2(safeStart, 0f);
        zone.anchorMax = new Vector2(safeEnd, 1f);

        zone.offsetMin = Vector2.zero;
        zone.offsetMax = Vector2.zero;
    }

    /*
    Responsabilidade:
    Ativar ou desativar o visual do buff da zona Perfect.

    Usado pela lata roxa.
    */
    public void SetBoostVisual(bool active)
    {
        if (perfectZoneOutline == null)
            return;

        perfectZoneOutline.enabled = active;
    }
}