/*
Responsabilidade:
Controlar o valor do ponteiro da barra de força/precisão.

Esse script NÃO desenha a UI.
Ele apenas calcula o valor atual da barra entre 0 e 1.

Valor:
- 0 = lado esquerdo da barra
- 1 = lado direito da barra

Regras:
- o ponteiro começa indo para a direita
- ao chegar em 1, volta para a esquerda
- ao chegar em 0, volta para a direita
- a velocidade aumenta a cada 10 segundos de partida

Dependências:
- TimerSystem: usado para calcular o tempo passado e aumentar a dificuldade
- GameplayLockSystem: impede movimento durante countdown/pause inicial
- ShotEvaluator: lê o value para decidir Miss, Good ou Perfect
- PowerBarUI: normalmente usa esse value para mover o ponteiro visual
*/

using UnityEngine;

public class PowerBar : MonoBehaviour
{
    [Header("Value")]
    [Range(0f, 1f)]
    public float value = 0f;

    [Header("Speed")]
    public float speed = 0.6f;

    [Header("Difficulty")]
    public float startSpeed = 0.6f;
    public float speedIncreaseEvery10Seconds = 0.35f;
    public float maxSpeed = 3f;

    [Header("References")]
    public TimerSystem timerSystem;

    [Header("State")]
    public bool goingRight = true;

    [Header("Debug")]
    public bool enableLogs = false;

    private int lastDifficultyStep = -1;

    private void Start()
    {
        ResetBar();
    }

    private void Update()
    {
        if (!CanMove())
            return;

        UpdateDifficulty();
        MovePointer();
    }

    /*
    Responsabilidade:
    Definir se a barra pode se mover agora.
    */
    private bool CanMove()
    {
        if (GameplayLockSystem.IsGameplayLocked)
            return false;

        if (timerSystem != null && timerSystem.isGameOver)
            return false;

        return true;
    }

    /*
    Responsabilidade:
    Atualizar dificuldade baseada no tempo de partida.

    A cada 10 segundos:
    - aumenta a velocidade da barra
    - respeita o limite máximo
    */
    private void UpdateDifficulty()
    {
        if (timerSystem == null)
            return;

        float elapsedTime =
            timerSystem.startTime - timerSystem.timeRemaining;

        int difficultyStep =
            Mathf.FloorToInt(elapsedTime / 10f);

        speed = startSpeed +
                (difficultyStep * speedIncreaseEvery10Seconds);

        speed = Mathf.Clamp(
            speed,
            startSpeed,
            maxSpeed
        );

        if (enableLogs && difficultyStep != lastDifficultyStep)
        {
            Debug.Log($"PowerBar | Speed: {speed}");
            lastDifficultyStep = difficultyStep;
        }
    }

    /*
    Responsabilidade:
    Mover o ponteiro entre 0 e 1.
    */
    private void MovePointer()
    {
        float direction = goingRight ? 1f : -1f;

        value += direction * speed * Time.deltaTime;

        if (value >= 1f)
        {
            value = 1f;
            goingRight = false;
            return;
        }

        if (value <= 0f)
        {
            value = 0f;
            goingRight = true;
        }
    }

    /*
    Responsabilidade:
    Resetar a barra para o estado inicial.
    */
    public void ResetBar()
    {
        value = 0f;
        speed = startSpeed;
        goingRight = true;
        lastDifficultyStep = -1;
    }
}