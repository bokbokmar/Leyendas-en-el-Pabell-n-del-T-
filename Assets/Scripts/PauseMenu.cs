using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausacoso;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausacoso.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
    }
public void seguircoso()
    {
        pausacoso.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
    }

    public void MainMenu()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }
}
