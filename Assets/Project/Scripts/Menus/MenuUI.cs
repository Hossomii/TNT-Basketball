using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    // Nome das cenas (igual no Build Settings)
    public int cenaJogo = 2;
    public int cenaRanking = 4;
    public int cenaPersonalizar = 0;
    public int cenaTutorial = 5;
    public int cenaCreditos = 6;
    public int trocaSkin = 1;

    public void IniciarJogo()
    {
        SceneManager.LoadScene(cenaJogo);
    }

    public void TrocarSkin()
    {
        SceneManager.LoadScene(trocaSkin);
    }

    public void AbrirRanking()
    {
        SceneManager.LoadScene(cenaRanking);
    }

    public void AbrirPersonalizar()
    {
        SceneManager.LoadScene(cenaPersonalizar);
    }

    public void AbrirTutorial()
    {
        SceneManager.LoadScene(cenaTutorial);
    }

    public void AbrirCreditos()
    {
        SceneManager.LoadScene(cenaCreditos);
    }

    public void SairJogo()
    {
        Debug.Log("Saiu do jogo");
        Application.Quit();
    }
}