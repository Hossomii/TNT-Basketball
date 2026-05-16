/*
Responsabilidade:
Definir qual música deve tocar ao entrar nesta cena.

Uso:
Coloque este script em um objeto vazio da cena
e arraste a música desejada no campo Scene Music.
*/

using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    [Header("Scene Music")]
    [SerializeField] private AudioClip sceneMusic;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("SceneMusicController: AudioManager não encontrado.");
            return;
        }

        if (sceneMusic == null)
        {
            Debug.LogWarning("SceneMusicController: Nenhuma música definida para esta cena.");
            return;
        }

        AudioManager.Instance.PlayMusic(sceneMusic);
    }
}