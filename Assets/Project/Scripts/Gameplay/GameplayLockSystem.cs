using System;
using UnityEngine;

public class GameplayLockSystem : MonoBehaviour
{
    public static bool IsGameplayLocked { get; private set; } = true;

    public static event Action OnGameplayLocked;
    public static event Action OnGameplayUnlocked;

    [Header("Initial State")]
    [SerializeField] private bool lockOnAwake = true;

    private void Awake()
    {
        if (lockOnAwake)
            LockGameplay();
        else
            UnlockGameplay();
    }

    public void LockGameplay()
    {
        IsGameplayLocked = true;
        OnGameplayLocked?.Invoke();
    }

    public void UnlockGameplay()
    {
        IsGameplayLocked = false;
        OnGameplayUnlocked?.Invoke();
    }
}