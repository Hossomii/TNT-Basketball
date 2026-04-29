using UnityEngine;

public static class TNTColors
{
    public static Color Red = Hex("#D50032");
    public static Color Black = Hex("#000000");
    public static Color White = Hex("#FFFFFF");

    public static Color Yellow = Hex("#FEDD00");
    public static Color Orange = Hex("#FF9800");

    // Versões para gameplay
    public static Color Good = Hex("#FF3B5C");     // vermelho vibrante
    public static Color Perfect = Hex("#FEDD00");  // amarelo TNT
    public static Color Miss = Hex("#D50032");     // vermelho padrão

    private static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}