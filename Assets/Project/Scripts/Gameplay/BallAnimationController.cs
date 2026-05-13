/*
Responsabilidade:
Controlar as animações da bola após o arremesso.

Esse script recebe o resultado do arremesso e toca:
- animação Good
- animação Perfect
- animação Miss

Também sincroniza o impacto da cesta no momento certo.

Dependências:
- Animator da bola
- BasketAnimationController
- InputHandler: espera essa coroutine terminar antes de liberar novo arremesso
- GameplayLockSystem: pausa animações durante countdown
*/

using System.Collections;
using UnityEngine;

public class BallAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public BasketAnimationController basket;

    [Header("Animation Total Delays")]
    public float goodDelay = 1.00f;
    public float perfectDelay = 0.80f;
    public float missDelay = 1.10f;

    [Header("Basket Impact Delays")]
    public float goodBasketDelay = 0.47f;
    public float perfectBasketDelay = 0.33f;
    public float missBasketDelay = 0.33f;

    [Header("Basket Impact")]
    public bool playBasketOnMiss = false;

    [Header("Animator Triggers")]
    public string goodTrigger = "Good";
    public string perfectTrigger = "Perfect";
    public string missTrigger = "Miss";

    private void Awake()
    {
        UpdateAnimatorLockState();
    }

    private void OnEnable()
    {
        GameplayLockSystem.OnGameplayLocked += PauseAnimator;
        GameplayLockSystem.OnGameplayUnlocked += ResumeAnimator;

        UpdateAnimatorLockState();
    }

    private void OnDisable()
    {
        GameplayLockSystem.OnGameplayLocked -= PauseAnimator;
        GameplayLockSystem.OnGameplayUnlocked -= ResumeAnimator;
    }

    /*
    Responsabilidade:
    Tocar a animação correspondente ao resultado.
    */
    public IEnumerator PlayResultAnimation(ShotEvaluator.ShotResult result)
    {
        if (!CanPlayAnimation())
            yield break;

        ResumeAnimator();

        switch (result)
        {
            case ShotEvaluator.ShotResult.Good:
                yield return PlayShotAnimation(
                    goodTrigger,
                    goodDelay,
                    goodBasketDelay,
                    true
                );
                break;

            case ShotEvaluator.ShotResult.Perfect:
                yield return PlayShotAnimation(
                    perfectTrigger,
                    perfectDelay,
                    perfectBasketDelay,
                    true
                );
                break;

            case ShotEvaluator.ShotResult.Miss:
                yield return PlayShotAnimation(
                    missTrigger,
                    missDelay,
                    missBasketDelay,
                    playBasketOnMiss
                );
                break;
        }
    }

    private bool CanPlayAnimation()
    {
        if (animator == null)
        {
            Debug.LogWarning("BallAnimationController: Animator da bola não atribuído.");
            return false;
        }

        if (GameplayLockSystem.IsGameplayLocked)
            return false;

        return true;
    }

    /*
    Responsabilidade:
    Tocar animação da bola e sincronizar a cesta.
    */
    private IEnumerator PlayShotAnimation(
        string triggerName,
        float totalDelay,
        float impactDelay,
        bool shouldPlayBasket
    )
    {
        PlayBallTrigger(triggerName);

        if (shouldPlayBasket && basket != null)
        {
            yield return new WaitForSeconds(impactDelay);

            basket.PlayBounce();

            float remainingDelay = totalDelay - impactDelay;

            if (remainingDelay > 0f)
                yield return new WaitForSeconds(remainingDelay);

            yield break;
        }

        yield return new WaitForSeconds(totalDelay);
    }

    private void PlayBallTrigger(string triggerName)
    {
        animator.ResetTrigger(triggerName);
        animator.SetTrigger(triggerName);
    }

    private void UpdateAnimatorLockState()
    {
        if (GameplayLockSystem.IsGameplayLocked)
            PauseAnimator();
        else
            ResumeAnimator();
    }

    private void PauseAnimator()
    {
        if (animator != null)
            animator.speed = 0f;
    }

    private void ResumeAnimator()
    {
        if (animator != null)
            animator.speed = 1f;
    }
}