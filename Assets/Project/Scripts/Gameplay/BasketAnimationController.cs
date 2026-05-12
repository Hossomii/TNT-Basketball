/*
Responsabilidade:
Controlar a animação da cesta/rede.

Esse script toca o bounce da cesta quando a bola acerta.

Dependências:
- BallAnimationController: chama PlayBounce()
- Animator da cesta/rede
*/

using UnityEngine;

public class BasketAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Animator")]
    public string bounceTrigger = "Bounce";

    [Header("Debug")]
    public bool enableLogs = false;

    /*
    Responsabilidade:
    Tocar animação de impacto da cesta.
    */
    public void PlayBounce()
    {
        if (animator == null)
        {
            Debug.LogWarning("BasketAnimationController: Animator não atribuído.");
            return;
        }

        animator.speed = 1f;

        animator.ResetTrigger(bounceTrigger);
        animator.SetTrigger(bounceTrigger);

        if (enableLogs)
            Debug.Log("BasketAnimationController: Bounce chamado.");
    }
}