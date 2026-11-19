using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameManager manager;
    public GameObject cliente;
    public InventarioJugador inventario;
    public Transform[] asientos;
    [SerializeField] private Transform[] mesas;
    public bool[] asientoTomado;
    public Transform puntoAparicion;

    public float tiempoRestante;
    public bool temporizadorActivo;
    public bool puedeGenerar;
    public int auxilia;
    public int ocupados;

    private void Start()
    {
        manager = gameObject.GetComponent<GameManager>();
        puntoAparicion = transform.GetChild(0);
        Sillitas();
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
            {
                tiempoRestante -= Time.deltaTime;
            }
            else
            {
                tiempoRestante = 0;
                temporizadorActivo = false;
            }
        }
    }

    public void Generar()
    {
        GameObject clon = Instantiate(cliente, puntoAparicion.position, transform.rotation);
        clon.GetComponent<Clientes>().inventario = inventario;
        clon.GetComponent<Clientes>().manager = manager;
        tiempoRestante = 8;
        temporizadorActivo = true;
    }

    public void RevisarAsientos()
    {
        ocupados = 0;
        foreach (bool asientito in asientoTomado)
        {
            if (asientito) { ocupados++; }
        }
        if (ocupados == 16)
        {
            puedeGenerar = false;
        }
        else
        {
            puedeGenerar = true;
        }
    }

    public void Sillitas()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                asientos[auxilia] = mesas[i].GetChild(j).transform;
                auxilia = auxilia + 1;
            }
        }
    }
}