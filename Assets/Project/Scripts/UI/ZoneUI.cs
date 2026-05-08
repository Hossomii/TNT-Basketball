/*
Responsabilidade:
Atualizar visualmente as zonas Good e Perfect na barra.

Usado por:
- ZoneRandomizer

Importante:
Esse script altera apenas UI.
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
    Atualizar zonas da UI.
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
    Atualizar anchors de uma zona.
    */
    private void UpdateZone(
        RectTransform zone,
        float start,
        float end
    )
    {
        if (zone == null)
            return;

        zone.anchorMin = new Vector2(start, 0f);
        zone.anchorMax = new Vector2(end, 1f);

        zone.offsetMin = Vector2.zero;
        zone.offsetMax = Vector2.zero;
    }

    /*
    Responsabilidade:
    Ativar visual do buff da zona Perfect.
    */
    public void SetBoostVisual(bool active)
    {
        if (perfectZoneOutline != null)
        {
            perfectZoneOutline.enabled = active;
        }
    }
}