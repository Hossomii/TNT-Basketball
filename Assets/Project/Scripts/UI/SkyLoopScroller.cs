/*
Responsabilidade:
Mover duas imagens do céu em loop horizontal.

Como funciona:
- Usa duas cópias do mesmo céu.
- Move ambas para a esquerda.
- Quando uma sai da tela, ela volta para a direita da outra.

Usado em:
- Background animado da cena Gameplay.

Dependências:
- Dois Transforms com SpriteRenderer do céu.
*/

using UnityEngine;

public class SkyLoopScroller : MonoBehaviour
{
    [Header("Sky Layers")]
    [SerializeField] private Transform skyA;
    [SerializeField] private Transform skyB;

    [Header("Settings")]
    [SerializeField] private float speed = 0.15f;
    [SerializeField] private float imageWidth = 19.2f;

    [Header("Direction")]
    [SerializeField] private bool moveLeft = true;



    private void Update()
    {
        if (GameplayLockSystem.IsGameplayLocked)
            return;

        MoveSky();
        RepositionIfNeeded();
    }

    private void MoveSky()
    {
        if (skyA == null || skyB == null)
            return;

        float direction = moveLeft ? -1f : 1f;
        float movement = direction * speed * Time.deltaTime;

        skyA.position += new Vector3(movement, 0f, 0f);
        skyB.position += new Vector3(movement, 0f, 0f);
    }

    private void RepositionIfNeeded()
    {
        if (skyA == null || skyB == null)
            return;

        if (moveLeft)
        {
            if (skyA.position.x <= -imageWidth)
                skyA.position = new Vector3(skyB.position.x + imageWidth, skyA.position.y, skyA.position.z);

            if (skyB.position.x <= -imageWidth)
                skyB.position = new Vector3(skyA.position.x + imageWidth, skyB.position.y, skyB.position.z);
        }
        else
        {
            if (skyA.position.x >= imageWidth)
                skyA.position = new Vector3(skyB.position.x - imageWidth, skyA.position.y, skyA.position.z);

            if (skyB.position.x >= imageWidth)
                skyB.position = new Vector3(skyA.position.x - imageWidth, skyB.position.y, skyB.position.z);
        }
    }
}