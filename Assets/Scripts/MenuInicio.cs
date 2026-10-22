using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    public string escenaJuego;

    public void Jugar()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    public void Salir()
    {
        Application.Quit();
    }
}