using UnityEngine;

public class ZoneUI : MonoBehaviour
{
    public RectTransform goodZone;
    public RectTransform perfectZone;

    public void UpdateZones(float goodStart, float goodEnd, float perfectStart, float perfectEnd)
    {
        UpdateZone(goodZone, goodStart, goodEnd);
        UpdateZone(perfectZone, perfectStart, perfectEnd);
    }

    private void UpdateZone(RectTransform zone, float start, float end)
    {
        zone.anchorMin = new Vector2(start, 0f);
        zone.anchorMax = new Vector2(end, 1f);

        zone.offsetMin = Vector2.zero;
        zone.offsetMax = Vector2.zero;
        zone.pivot = new Vector2(0.5f, 0.5f);
    }
}