/*
Responsabilidade:
Controlar o estado global da gameplay.

Esse sistema define se o jogo está:
- bloqueado
- liberado

Quando bloqueado:
- input pode parar
- timers podem parar
- animações podem pausar
- elementos de UI podem congelar

Dependências:
- GameStartCountdown
- TimerSystem
- LockableAnimator
- TNTCanVisualController

Fluxo:
LockGameplay()
-> ativa IsGameplayLocked
-> dispara evento OnGameplayLocked

UnlockGameplay()
-> desativa IsGameplayLocked
-> dispara evento OnGameplayUnlocked
*/

using System;
using UnityEngine;

public class GameplayLockSystem : MonoBehaviour
{
    /*
    Estado global da gameplay.

    TRUE:
    gameplay bloqueada

    FALSE:
    gameplay liberada
    */
    public static bool IsGameplayLocked { get; private set; } = true;

    /*
    Eventos globais.

    Outros scripts podem escutar:
    - quando gameplay trava
    - quando gameplay libera
    */
    public static event Action OnGameplayLocked;
    public static event Action OnGameplayUnlocked;

    [Header("Initial State")]
    [SerializeField]
    private bool lockOnAwake = true;

    private void Awake()
    {
        if (lockOnAwake)
            LockGameplay();
        else
            UnlockGameplay();
    }

    /*
    Responsabilidade:
    Bloquear gameplay globalmente.
    */
    public void LockGameplay()
    {
        // Evita chamadas duplicadas.
        if (IsGameplayLocked)
            return;

        IsGameplayLocked = true;

        OnGameplayLocked?.Invoke();
    }

    /*
    Responsabilidade:
    Liberar gameplay globalmente.
    */
    public void UnlockGameplay()
    {
        // Evita chamadas duplicadas.
        if (!IsGameplayLocked)
            return;

        IsGameplayLocked = false;

        OnGameplayUnlocked?.Invoke();
    }
}