using UnityEngine;

public class Mesa : MonoBehaviour
{
    [Header("Qué acepta")]
    public string[] idsAceptados = { "TeVerde", "TeBlanco", "TeRojo" };

    [Header("Colocación")]
    public Transform puntoColocacion;

    [Header("Desaparición")]
    public float tiempoEnMesa = 10f;
    public GameObject efectoDesaparicion;
    public GameObject efectoColocacion;

    private ItemRecogible itemEnMesa;
    private float temporizador;
    private Clientes clienteAsignado;

    public bool MesaOcupada => itemEnMesa != null;

    private void Update()
    {
        if (itemEnMesa == null) return;

        temporizador -= Time.deltaTime;

        if (temporizador <= 0f)
            DesaparecerItem();
    }

    public bool Colocar(ItemRecogible item, InventarioJugador inventario)
    {
        if (item == null) return false;

        if (MesaOcupada)
        {
            Debug.Log("Mesa: ya hay un item");
            return false;
        }

        bool aceptado = false;
        foreach (string id in idsAceptados)
        {
            if (item.idIngrediente == id) { aceptado = true; break; }
        }
        if (!aceptado)
        {
            Debug.Log($"Mesa: '{item.idIngrediente}' no es un té válido");
            return false;
        }

        itemEnMesa = item;

        if (puntoColocacion != null)
        {
            item.transform.SetParent(puntoColocacion);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }
        else
        {
            item.transform.SetParent(transform);
            item.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }

        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (item.TryGetComponent<Collider>(out Collider col))
            col.enabled = false;

        temporizador = tiempoEnMesa;

        if (efectoColocacion != null)
            Instantiate(efectoColocacion, item.transform.position, Quaternion.identity);

        Debug.Log($"Mesa: '{item.nombreItem}' colocado. Esperando cliente...");
        return true;
    }

    
    public bool ClienteRecogeTe(int tipoDeseado, Clientes cliente)
    {
        if (itemEnMesa == null)
        {
            Debug.Log("Mesa: no hay té para recoger");
            return false;
        }

        int tipoColocado = IdToTipo(itemEnMesa.idIngrediente);

        bool esCorrecto = (tipoColocado == tipoDeseado);

        Debug.Log($"Mesa: cliente recoge '{itemEnMesa.nombreItem}'. Correcto: {esCorrecto}");

        if (efectoDesaparicion != null)
            Instantiate(efectoDesaparicion, itemEnMesa.transform.position, Quaternion.identity);

        Destroy(itemEnMesa.gameObject);
        itemEnMesa = null;
        temporizador = 0f;

        return esCorrecto;
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

    private void DesaparecerItem()
    {
        if (itemEnMesa == null) return;

        Debug.Log($"Mesa: '{itemEnMesa.nombreItem}' desaparece (nadie lo recogió)");

        if (efectoDesaparicion != null)
            Instantiate(efectoDesaparicion, itemEnMesa.transform.position, Quaternion.identity);

        Destroy(itemEnMesa.gameObject);
        itemEnMesa = null;
        temporizador = 0f;
    }
}