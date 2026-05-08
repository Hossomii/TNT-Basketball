/*
Responsabilidade:
Controlar animação da cesta.

Usado por:
- BallAnimationController
*/

using UnityEngine;

public class BasketAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Animator")]
    public string bounceTrigger = "Bounce";

    /*
    Responsabilidade:
    Disparar animação de bounce da cesta.
    */
    public void PlayBounce()
    {
        if (animator == null)
            return;

        animator.SetTrigger(bounceTrigger);
    }
}