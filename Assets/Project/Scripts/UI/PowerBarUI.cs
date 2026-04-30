using UnityEngine;

public class PowerBarUI : MonoBehaviour
{
    public PowerBar powerBar;
    public RectTransform aimPointer;

    [Header("Visual Offset")]
    public Vector2 pointerOffset = new Vector2(0f, 10f);
    public Vector2 pointerSize = new Vector2(32f, 32f);

    private void Update()
    {
        if (powerBar == null || aimPointer == null)
            return;

        float value = powerBar.value;

        aimPointer.anchorMin = new Vector2(value, 0f);
        aimPointer.anchorMax = new Vector2(value, 0f);

        aimPointer.pivot = new Vector2(0.5f, 0f);
        aimPointer.sizeDelta = new Vector2(52f, 52f);
        aimPointer.anchoredPosition = pointerOffset;
    }
}

/*
Responsabilidade:
Controlar a posição visual da seta (AimPointer) na PowerBar.

Como funciona:
- Lê o valor atual da barra (powerBar.value), que varia de 0 a 1.
- Usa esse valor para posicionar a seta horizontalmente na barra.
- Mantém a seta fixa em tamanho e formato.
- Aplica um offset manual (pointerOffset) para ajustar a posição visual.

Interações:
- Recebe dados do PowerBar (valor atual da barra).
- NÃO interfere na lógica do jogo, apenas na UI (visual).

Importante:
- A posição horizontal da seta é controlada pelos anchors (0 a 1).
- A posição vertical e pequenos ajustes são controlados pelo pointerOffset.

PointerOffset:
- X: ajuste horizontal (geralmente não usado)
- Y: controla a altura da seta em relação à barra
  -> valores maiores sobem a seta
  -> valores menores descem a seta

Observação:
A posição definida no Inspector é ignorada durante o jogo,
pois o script atualiza a posição da seta a cada frame.
*/