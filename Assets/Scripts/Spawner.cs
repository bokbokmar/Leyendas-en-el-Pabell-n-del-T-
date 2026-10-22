using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject cliente;

    public Transform[] asientos;
    public bool[] asientoTomado;
    public Transform puntoAparicion;

    public float tiempoRestante;
    public bool temporizadorActivo;
    public bool puedeGenerar;

    private void Start()
    {
        puntoAparicion = transform.GetChild(0);
    }

    private void Update()
    {
        RevisarAsientos();

        if (puedeGenerar && !temporizadorActivo)
        {
            Generar();
        }
        else if (temporizadorActivo)
        {
            if (tiempoRestante > 0)
                tiempoRestante -= Time.deltaTime;
            else
            {
                tiempoRestante = 0;
                temporizadorActivo = false;
            }
        }
    }

    public void Generar()
    {
        Instantiate(cliente, puntoAparicion.position, transform.rotation);
        tiempoRestante = 3;
        temporizadorActivo = true;
    }

    public void RevisarAsientos()
    {
        int ocupados = 0;
        foreach (bool asientito in asientoTomado)
        {
            if (asientito) ocupados++;
        }
        puedeGenerar = ocupados < 4;
    }
}