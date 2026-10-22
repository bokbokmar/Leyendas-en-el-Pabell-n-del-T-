using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject menuPausa;
    [SerializeField] private bool juegoPausado;
    public string menuPrincipal;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
                Reanudar();
            else
                Pausa();
        }
    }

    public void Pausa()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuPrincipal);
    }
}