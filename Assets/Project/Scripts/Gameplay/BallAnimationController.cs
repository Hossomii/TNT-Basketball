using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;

public class BallAnimationController : MonoBehaviour
{
    public Animator animator;
    public BasketAnimationController basket;
    public AudioSource audioSource;
    public AudioClip ballSFX;

    public float goodDelay = 0.9f;
    public float perfectDelay = 0.7f;
    public float missDelay = 0.5f;

    public float basketDelay = 0.3f;

    public IEnumerator PlayResultAnimation(ShotEvaluator.ShotResult result)
    {
        if (animator == null)
            yield break;

        PlayBallSFX();

        switch (result)
        {
            case ShotEvaluator.ShotResult.Good:
                animator.SetTrigger("Good");
                yield return new WaitForSeconds(basketDelay);
                basket?.PlayBounce();
                yield return new WaitForSeconds(goodDelay);
                break;

            case ShotEvaluator.ShotResult.Perfect:
                animator.SetTrigger("Perfect");
                yield return new WaitForSeconds(basketDelay);
                basket?.PlayBounce();
                yield return new WaitForSeconds(perfectDelay);
                break;

            case ShotEvaluator.ShotResult.Miss:
                animator.SetTrigger("Miss");
                yield return new WaitForSeconds(missDelay);
                break;
        }
    }

    private void PlayBallSFX()     
    {
        if (audioSource != null && ballSFX != null)
        {
            audioSource.PlayOneShot(ballSFX);
        }
    }
}