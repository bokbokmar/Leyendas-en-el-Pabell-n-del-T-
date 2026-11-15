using UnityEngine;
using UnityEngine.AI;

public class Clientes : MonoBehaviour, IInteractuable
{
    public int teDeseado;
    public int auxiliari;
    public bool clienteSatisfecho;
    public bool clienteInsatisfecho;
    public bool clienteAtendido;
    public bool sentado;
    public bool asientoElegido;

    public float tiempoEspera = 30f;
    public bool esperando;

    public GameObject[] iconos; //Del 0, 1 y 2 son las hojas de té. El espacio 3 será el icono de espera

    public Spawner generador;

    public Transform objetivo;
    public Transform salida;
    private NavMeshAgent agente;
    public GameObject iconoEspera;

    [SerializeField] private GameObject moneda;
    public InventarioJugador inventario;
    public GameObject teRecibido;
    public float numerito;

    private void Start()
    {
        sentado = false;
        esperando = false;
        iconos = new GameObject[4];
        agente = GetComponent<NavMeshAgent>();

        generador = GameObject.Find("Spawner de Clientes").GetComponent<Spawner>();
        for (int i = 0; i < transform.childCount; i++)
        {
            iconos[i] = transform.GetChild(i).gameObject;
        }

        ElegirAsiento();
    }

    private void Update()
    {
        numerito = agente.remainingDistance;
        if (agente.remainingDistance < 0.1f)
        {
            sentado = true;
            esperando = true;
        }

        if (esperando)
        {
            //float tiempoAux = tiempoEspera;
            //tiempoAux -= Time.deltaTime;
            tiempoEspera -= Time.deltaTime;

            if (tiempoEspera <= 0)
            {
                esperando = false;
                if (!clienteSatisfecho)
                {
                    clienteInsatisfecho = true;
                    Irse();
                }
                tiempoEspera = 30f;
            }
        }

        if (clienteSatisfecho || clienteInsatisfecho)
        {
            Irse();
        }

    }

    public void Interactuar()
    {
        if (!agente.pathPending && !clienteAtendido)
        {
            ElegirTe();
        }

        if (clienteAtendido)
        {
            RecibirTe();
        }
    }

    public void ElegirTe()
    {
        teDeseado = Random.Range(0, 3);
        clienteAtendido = true;
        tiempoEspera = 30f;

        for (int i = 0; i < iconos.Length; i++)
        {
            iconos[i].SetActive(false);
        }

        iconos[teDeseado].SetActive(true);

        if (iconoEspera != null)
        {
            iconoEspera.SetActive(false);
        }
        //Debug.Log($"Cliente quiere té tipo {teDeseado}");
    }

    public void Sentarse()
    {
        if (asientoElegido)
        {
            agente.SetDestination(objetivo.position);
        }
    }

    public void ElegirAsiento()
    {
        auxiliari = Random.Range(0, 16);
        if (generador.asientoTomado[auxiliari])
        {
            ElegirAsiento();
        }
        else
        {
            asientoElegido = true;
            objetivo = generador.asientos[auxiliari];
            Sentarse();
        }
    }

    public void Irse()
    {
        bool aux2 = false;
        generador.ocupados--;
        salida = generador.gameObject.transform.GetChild(1);
        agente.SetDestination(salida.position);

        foreach (GameObject iconitos in iconos)
        {
            iconitos.SetActive(false);
        }

        if (clienteSatisfecho && !clienteInsatisfecho)
        {
            //Instantiate(moneda, transform.forward * 30, transform.rotation);
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if (!clienteSatisfecho && clienteInsatisfecho)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    public void RecibirTe()
    {
        if (inventario.manoJugador.childCount < 1)
        {
            Debug.Log("No traes te");
            return;
        }
        else
        {
            string aux = inventario.objetoEnMano.GetComponent<ItemRecogible>().nombreItem;
            teRecibido = inventario.objetoEnMano;
            if (inventario.objetoEnMano.GetComponent<ItemRecogible>().idIngrediente == teDeseado)
            {
                clienteSatisfecho = true;
            }
            else
            {
                clienteInsatisfecho = true;
            }
        }
    }
}