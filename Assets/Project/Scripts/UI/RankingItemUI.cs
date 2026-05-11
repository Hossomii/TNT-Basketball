using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingItemUI : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text nameText;
    public TMP_Text scoreText;
    public Image background;

    public Color firstColor;
    public Color secondColor;
    public Color thirdColor;
    public Color normalColor;

    public void Setup(int rank, string playerName, int score)
    {
        rankText.text = rank.ToString();
        nameText.text = playerName;
        scoreText.text = score.ToString();

        if (background != null)
        {
            if (rank == 1)
                background.color = firstColor;
            else if (rank == 2)
                background.color = secondColor;
            else if (rank == 3)
                background.color = thirdColor;
            else
                background.color = normalColor;
        }
    }
}