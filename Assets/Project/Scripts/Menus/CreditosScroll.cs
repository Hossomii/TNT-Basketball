using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditosScroll : MonoBehaviour
{
    public float velocidade = 50f;
    public float limiteY = 1000f;
    public int cenaMenu = 1;

    void Update()
    {
        transform.Translate(Vector3.up * velocidade * Time.deltaTime);

        if(transform.position.y > limiteY)
        {
            SceneManager.LoadScene(cenaMenu);
        }
    }
}