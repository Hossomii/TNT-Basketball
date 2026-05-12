/*
Responsabilidade:
Controlar o visual do ponteiro da PowerBar.

Esse script NÃO calcula gameplay.
Ele apenas lê o valor atual da PowerBar
e move visualmente o AimPointer na UI.

Fluxo:
PowerBar.value
-> PowerBarUI
-> move AimPointer

Importante:
Esse script força:
- scale = 1
- size original
- anchors corretas

Isso evita bugs visuais como:
- ponteiro esticado
- ponteiro achatado
- scale herdado de parents
- deformações durante gameplay

Dependências:
- PowerBar: fornece o valor atual entre 0 e 1
- RectTransform da barra
- RectTransform do ponteiro
*/

using UnityEngine;

public class PowerBarUI : MonoBehaviour
{
    [Header("References")]
    public PowerBar powerBar;
    public RectTransform barRect;
    public RectTransform aimPointer;

    [Header("Visual")]
    public Vector2 pointerOffset = new Vector2(0f, 10f);

    private Vector2 originalSize;

    private void Start()
    {
        SetupPointer();
        UpdatePointerPosition();
    }

    private void Update()
    {
        if (!CanUpdate())
            return;

        UpdatePointerPosition();
    }

    /*
    Responsabilidade:
    Validar referências necessárias.
    */
    private bool CanUpdate()
    {
        return powerBar != null &&
               barRect != null &&
               aimPointer != null;
    }

    /*
    Responsabilidade:
    Configurar o ponteiro para um estado estável.

    Corrige:
    - anchors
    - pivot
    - scale herdado
    - tamanho original
    */
    private void SetupPointer()
    {
        if (aimPointer == null)
            return;

        originalSize = aimPointer.sizeDelta;

        aimPointer.anchorMin = new Vector2(0f, 0.5f);
        aimPointer.anchorMax = new Vector2(0f, 0.5f);

        aimPointer.pivot = new Vector2(0.5f, 0.5f);

        aimPointer.localScale = Vector3.one;
        aimPointer.sizeDelta = originalSize;
    }

    /*
    Responsabilidade:
    Atualizar posição visual do ponteiro.
    */
    private void UpdatePointerPosition()
    {
        float normalizedValue =
            Mathf.Clamp01(powerBar.value);

        float barWidth =
            barRect.rect.width;

        float xPosition =
            normalizedValue * barWidth;

        aimPointer.anchoredPosition = new Vector2(
            xPosition + pointerOffset.x,
            pointerOffset.y
        );

        MaintainPointerVisualStability();
    }

    /*
    Responsabilidade:
    Garantir que o ponteiro nunca seja deformado.

    Isso protege contra:
    - scale herdado
    - animações externas
    - alterações de layout
    */
    private void MaintainPointerVisualStability()
    {
        aimPointer.localScale = Vector3.one;
        aimPointer.sizeDelta = originalSize;
    }
}