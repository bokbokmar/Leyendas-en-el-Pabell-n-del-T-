using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool jugando;
    [SerializeField] private float tiempoJuego;
    private float tiempoRestante;
    public int dinerito;
    [SerializeField] private int cuota;
    [SerializeField] private TextMeshProUGUI reloj;
    [SerializeField] private TextMeshProUGUI dineroActual;
    [SerializeField] private GameObject victoria;
    [SerializeField] private GameObject derrota;

    private void Start()
    {
        tiempoRestante = tiempoJuego;
        //dinerito = 0;
        jugando = true;
    }

    private void Update()
    {
        //Reloj();
        if (jugando)
        {
            if (tiempoRestante > 0f)
            {
                tiempoRestante -= Time.deltaTime;
                ActualizarTexto();
            }
            else
            {
                jugando = false;
                tiempoRestante = 0f;
                ActualizarTexto();
                FinDelJuego();
            }
        }
    }

    public void Reloj()
    {
        if (jugando)
        {
            if (tiempoRestante > 0f)
            {
                tiempoRestante -= Time.deltaTime;
                ActualizarTexto();
            }
            else
            {
                jugando = false;
                FinDelJuego();
            }
        }
    }

    public void ActualizarTexto()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        reloj.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void FinDelJuego()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        if (dinerito >= cuota)
        {
            //Victoria
            derrota.SetActive(false);
            victoria.SetActive(true);
        }
        else
        {
            //Derrota
            victoria.SetActive(false);
            derrota.SetActive(true);
        }
    }

    public void ActualizarDinero()
    {
        dineroActual.text = dinerito.ToString();
    }
}
