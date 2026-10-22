using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Iniciar()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
