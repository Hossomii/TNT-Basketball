/*
Responsabilidade:
Pausar e retomar um Animator baseado no bloqueio do gameplay.

Use em:
- latinhas da UI
- próxima lata
- animações decorativas que não devem rodar durante o countdown
*/

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LockableAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameplayLockSystem.OnGameplayLocked += PauseAnimator;
        GameplayLockSystem.OnGameplayUnlocked += ResumeAnimator;

        if (GameplayLockSystem.IsGameplayLocked)
            PauseAnimator();
        else
            ResumeAnimator();
    }

    private void OnDisable()
    {
        GameplayLockSystem.OnGameplayLocked -= PauseAnimator;
        GameplayLockSystem.OnGameplayUnlocked -= ResumeAnimator;
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