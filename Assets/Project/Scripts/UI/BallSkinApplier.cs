/*
Responsabilidade:
Aplicar skin selecionada na bola da gameplay.

Como funciona:
- Lê a skin salva no PlayerPrefs.
- Troca o AnimatorController da bola.

Usado na cena:
- Gameplay
*/

using UnityEngine;

public class BallSkinApplier : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Skins")]
    public RuntimeAnimatorController[] skins;

    [Header("PlayerPrefs")]
    public string saveKey = "SelectedBallSkin";

    private void Start()
    {
        ApplySavedSkin();
    }

    /*
    Responsabilidade:
    Aplicar skin salva anteriormente.
    */
    private void ApplySavedSkin()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator não encontrado.");
            return;
        }

        if (skins == null || skins.Length == 0)
        {
            Debug.LogWarning("Nenhuma skin configurada.");
            return;
        }

        int skinIndex =
            PlayerPrefs.GetInt(saveKey, 0);

        skinIndex =
            Mathf.Clamp(
                skinIndex,
                0,
                skins.Length - 1
            );

        animator.runtimeAnimatorController =
            skins[skinIndex];
    }
}