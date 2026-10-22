using TMPro;
using UnityEngine;

public class RelojInterfaz : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI relojIn;
    [SerializeField] private bool enCurso = true;
    public float tiempoRestante;
    public int dineros = 0;

    [SerializeField] private GameObject victoria;
    [SerializeField] private GameObject derrota;

    private void Update()
    {
        if (enCurso)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime;
                ActualizarTexto();
            }
            else
            {
                tiempoRestante = 0;
                enCurso = false;
                FinalizarJuego();
            }
        }
    }

    private void FinalizarJuego()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        if (dineros > 50)
            victoria.SetActive(true);
        else
            derrota.SetActive(true);
    }

    private void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        relojIn.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}