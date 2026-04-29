using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    // Nome das cenas (igual no Build Settings)
    public int cenaJogo = 2;
    public int cenaRanking = 0;
    public int cenaPersonalizar = 3;
    public int cenaTutorial = 1;
    public int cenaCreditos = 4;

    public void IniciarJogo()
    {
        SceneManager.LoadScene(cenaJogo);
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