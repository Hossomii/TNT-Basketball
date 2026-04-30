using UnityEngine;

public class BasketAnimationController : MonoBehaviour
{
    public Animator animator;

    public void PlayBounce()
    {
        if (animator != null)
            animator.SetTrigger("Bounce");
    }
}