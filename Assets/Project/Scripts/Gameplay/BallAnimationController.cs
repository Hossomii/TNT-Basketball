/*
Responsabilidade:
Controlar animações visuais da bola.

Como funciona:
- Recebe o resultado do arremesso.
- Dispara animação de Good, Perfect ou Miss.
- Sincroniza o bounce da cesta em acertos.
- Aguarda o delay antes de liberar nova jogada.

Usado por:
- InputHandler
*/

using System.Collections;
using UnityEngine;

public class BallAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public BasketAnimationController basket;

    [Header("Animation Delays")]
    public float goodDelay = 0.9f;
    public float perfectDelay = 0.7f;
    public float missDelay = 0.5f;
    public float basketDelay = 0.3f;

    [Header("Animator Triggers")]
    public string goodTrigger = "Good";
    public string perfectTrigger = "Perfect";
    public string missTrigger = "Miss";

    public IEnumerator PlayResultAnimation(ShotEvaluator.ShotResult result)
    {
        if (animator == null)
            yield break;

        switch (result)
        {
            case ShotEvaluator.ShotResult.Good:
                yield return PlayHitAnimation(goodTrigger, goodDelay);
                break;

            case ShotEvaluator.ShotResult.Perfect:
                yield return PlayHitAnimation(perfectTrigger, perfectDelay);
                break;

            case ShotEvaluator.ShotResult.Miss:
                animator.SetTrigger(missTrigger);
                yield return new WaitForSeconds(missDelay);
                break;
        }
    }

    private IEnumerator PlayHitAnimation(string triggerName, float delay)
    {
        animator.SetTrigger(triggerName);

        yield return new WaitForSeconds(basketDelay);

        if (basket != null)
            basket.PlayBounce();

        yield return new WaitForSeconds(delay);
    }
}