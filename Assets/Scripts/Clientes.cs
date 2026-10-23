using UnityEngine;
using UnityEngine.AI;

public class Clientes : MonoBehaviour
{
    public int teDeseado;          
    public int auxiliarsoide;
    public bool clienteSatisfecho;
    public bool clienteInsatisfecho;
    public bool clienteAtendido;   
    public bool sentado;
    public bool asientoElegido;

    public float tiempoEspera = 30f;
    public bool esperando;

    public GameObject[] hojas;     

    public Spawner generador;
    public Mesa mesa;              

    public Transform objetivo;
    public Transform salida;
    private NavMeshAgent agente;
    public GameObject iconoEspera;

    [SerializeField] private GameObject moneda;

    private void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        auxiliarsoide = Random.Range(0, 4);
        generador = GameObject.Find("Spawner de Clientes").GetComponent<Spawner>();

        if (mesa == null)
            mesa = FindObjectOfType<Mesa>();
    }

    private void Update()
    {
        ElegirAsiento();

        if (iconoEspera != null)
        {
            if (sentado && !clienteAtendido)
                iconoEspera.SetActive(true);
            else
                iconoEspera.SetActive(false);
        }

        if (!agente.pathPending)
        {
            if (agente.remainingDistance <= agente.stoppingDistance)
            {
                if (!agente.hasPath)
                {
                    sentado = true;
                    esperando = true;

                    if (!clienteAtendido)
                        ElegirTe();
                }
            }
        }
        else
        {
            sentado = false;
        }

        if (esperando)
        {
            if (tiempoEspera > 0)
                tiempoEspera -= Time.deltaTime;
            else
            {
                tiempoEspera = 0;
                esperando = false;
                clienteInsatisfecho = true;
                hojas[teDeseado].SetActive(false);
                Irse();
            }
        }
    }


    private int IdToTipo(string id)
    {
        switch (id)
        {
            case "TeVerde": return 0;
            case "TeBlanco": return 1;
            case "TeRojo": return 2;
            default: return -1;
        }
    }

    public void ElegirTe()
    {
        teDeseado = Random.Range(0, 3);
        clienteAtendido = true;

        for (int i = 0; i < hojas.Length; i++)
            hojas[i].SetActive(false);

        hojas[teDeseado].SetActive(true);

        if (iconoEspera != null)
            iconoEspera.SetActive(false);

        Debug.Log($"Cliente quiere té tipo {teDeseado}");
    }


    public void RecibirTeDirecto(ItemRecogible item)
    {
        if (item == null) return;

        int tipoEntregado = IdToTipo(item.idIngrediente);

        for (int i = 0; i < hojas.Length; i++)
            hojas[i].SetActive(false);
        if (iconoEspera != null)
            iconoEspera.SetActive(false);

        if (tipoEntregado == teDeseado)
        {
            clienteSatisfecho = true;
            clienteInsatisfecho = false;
            Debug.Log($"Cliente satisfecho: quería {teDeseado}, recibió {tipoEntregado}");
        }
        else
        {
            clienteSatisfecho = false;
            clienteInsatisfecho = true;
            Debug.Log($"Cliente insatisfecho: quería {teDeseado}, recibió {tipoEntregado}");
        }

        esperando = false;
        Irse();

        Destroy(item.gameObject);
    }


    public void IrARecogerTe()
    {
        if (mesa == null) return;

        agente.SetDestination(mesa.transform.position);

        Invoke(nameof(IntentarRecogerDeMesa), 2f);
    }

    private void IntentarRecogerDeMesa()
    {
        if (mesa == null) return;

        bool correcto = mesa.ClienteRecogeTe(teDeseado, this);

        for (int i = 0; i < hojas.Length; i++)
            hojas[i].SetActive(false);
        if (iconoEspera != null)
            iconoEspera.SetActive(false);

        if (correcto)
        {
            clienteSatisfecho = true;
            clienteInsatisfecho = false;
        }
        else
        {
            clienteSatisfecho = false;
            clienteInsatisfecho = true;
        }

        esperando = false;
        Irse();
    }

    public void Sentarse()
    {
        if (asientoElegido)
            agente.SetDestination(objetivo.position);
    }

    public void ElegirAsiento()
    {
        if (!generador.asientoTomado[auxiliarsoide])
        {
            generador.asientoTomado[auxiliarsoide] = true;
            objetivo = generador.asientos[auxiliarsoide];
            asientoElegido = true;
            Sentarse();
        }
        else if (!asientoElegido)
        {
            auxiliarsoide = Random.Range(0, 4);
        }
    }

    public void Irse()
    {
        salida = generador.transform.GetChild(1);
        agente.SetDestination(salida.position);

        if (clienteSatisfecho && !clienteInsatisfecho)
        {
            Instantiate(moneda, objetivo.GetChild(0).position, objetivo.GetChild(0).rotation);
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if (!clienteSatisfecho && clienteInsatisfecho)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}