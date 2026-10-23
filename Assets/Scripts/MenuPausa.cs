using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject menuPausa;
    public string menuPrincipal;

    [Header("Scripts a desactivar al pausar")]
    public MonoBehaviour[] scriptsAJugador;

    private bool juegoPausado;

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

        foreach (MonoBehaviour s in scriptsAJugador)
        {
            if (s != null) s.enabled = false;
        }
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (MonoBehaviour s in scriptsAJugador)
        {
            if (s != null) s.enabled = true;
        }
    }

    public void MenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuPrincipal);
    }
}