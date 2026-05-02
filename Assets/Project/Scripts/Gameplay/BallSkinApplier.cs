using UnityEngine;

public class BallSkinApplier : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController[] skins;

    private void Start()
    {
        int skinIndex = PlayerPrefs.GetInt("SelectedBallSkin", 0);

        if (skinIndex >= 0 && skinIndex < skins.Length)
        {
            animator.runtimeAnimatorController = skins[skinIndex];
        }
    }
}