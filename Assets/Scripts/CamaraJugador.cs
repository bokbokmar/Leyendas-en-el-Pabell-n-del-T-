using UnityEngine;

public class CamaraJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float sensibilidad = 3f;
    public Transform cuerpoJugador;

    [Header("Interacción")]
    public int distancia = 4;
    public LayerMask ignorar;
    public float distanciaMaximaInteraccion = 3f;
    public KeyCode teclaInteractuar = KeyCode.E;
    public KeyCode teclaSoltar = KeyCode.Q;
    public KeyCode teclaRecuperar = KeyCode.R;

    private float rotacionX = 0f;
    private float rotacionY = 0f;

    private InventarioJugador inventario;
    private Dispensador dispensadorApuntado;
    private ZonaEntrega zonaApuntada;
    private ItemRecogible itemSueloApuntado;
    private Mesa mesaApuntada;
    private Clientes clienteApuntado;
    private Monedas monedaApuntada;




    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventario = GetComponentInParent<InventarioJugador>();
    }

    private void Update()
    {
        MoverCamara();
        DetectarObjetivo();
        IntentarInteractuar();
        IntentarSoltar();
        IntentarRecuperarIngredientes();
    }

    private void MoverCamara()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);
        rotacionY += mouseX;

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        cuerpoJugador.localRotation = Quaternion.Euler(0f, rotacionY, 0f);
    }

    private void DetectarObjetivo()
    {
        dispensadorApuntado = null;
        zonaApuntada = null;
        itemSueloApuntado = null;
        mesaApuntada = null;

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, distancia, ~ignorar))
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);

            if (dist <= distanciaMaximaInteraccion)
            {
                if (hit.transform.TryGetComponent<Dispensador>(out Dispensador disp))
                    dispensadorApuntado = disp;

                if (hit.transform.TryGetComponent<ZonaEntrega>(out ZonaEntrega zona))
                    zonaApuntada = zona;

            

                if (hit.transform.TryGetComponent<Mesa>(out Mesa mesa))
                    mesaApuntada = mesa;

                if (hit.transform.TryGetComponent<ItemRecogible>(out ItemRecogible item))
                {
                    if (item.transform.parent == null)
                        itemSueloApuntado = item;
                }
            }
        }

        if (hit.transform.TryGetComponent<Clientes>(out Clientes cli))
            clienteApuntado = cli;
        
        if (hit.transform.TryGetComponent<Monedas>(out Monedas moneda))
            monedaApuntada = moneda;

    }

    private void IntentarInteractuar()
    {
        if (!Input.GetKeyDown(teclaInteractuar)) return;
        if (inventario == null) return;

        if (dispensadorApuntado != null)
        {
            if (!inventario.TieneEspacio()) { Debug.Log("Inventario lleno"); return; }
            GameObject nuevoItem = dispensadorApuntado.EntregarItem();
            if (nuevoItem != null) inventario.AgregarItem(nuevoItem);
            return;
        }

        if (zonaApuntada != null)
        {
            if (inventario.CantidadItems() == 0) { Debug.Log("No tienes items"); return; }
            ItemRecogible item = inventario.SacarUltimoItem();
            if (item != null)
            {
                bool ok = zonaApuntada.Entregar(item, inventario);
                if (!ok) inventario.AgregarItem(item.gameObject);
            }
            return;
        }

        if (itemSueloApuntado != null)
        {
            if (!inventario.TieneEspacio()) { Debug.Log("Inventario lleno"); return; }

            GameObject itemGO = itemSueloApuntado.gameObject;
            itemSueloApuntado = null;

            inventario.AgregarItem(itemGO);
            Debug.Log("Item recogido del suelo");
            return;
        }

       
        if (mesaApuntada != null)
        {
            if (inventario.CantidadItems() == 0) { Debug.Log("No tienes items"); return; }
            ItemRecogible item = inventario.SacarUltimoItem();
            if (item != null)
            {
                bool ok = mesaApuntada.Colocar(item, inventario);
                if (!ok) inventario.AgregarItem(item.gameObject); 
            }
            return;
        }

        if (clienteApuntado != null)
        {
            if (inventario.CantidadItems() == 0) { Debug.Log("No tienes items"); return; }
            ItemRecogible item = inventario.SacarUltimoItem();
            if (item != null)
            {
                int tipo = IdToTipoTe(item.idIngrediente);
                if (tipo >= 0)
                    clienteApuntado.RecibirTeDirecto(tipo);
                else
                {
                    Debug.Log("Eso no es un té");
                    inventario.AgregarItem(item.gameObject);
                }
            }
            return;
        }

        if (monedaApuntada != null)
        {
            monedaApuntada.Recoger();
            monedaApuntada = null;
            return;
        }

    }

    private void IntentarSoltar()
    {
        if (!Input.GetKeyDown(teclaSoltar)) return;
        if (inventario == null || inventario.CantidadItems() == 0) return;

        ItemRecogible item = inventario.SacarUltimoItem();
        if (item == null) return;

        Vector3 posicion = transform.position + transform.forward * 1.5f;
        item.transform.position = posicion;
        item.transform.rotation = Quaternion.identity;
        item.transform.SetParent(null);

        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (item.TryGetComponent<Collider>(out Collider col))
            col.enabled = true;

        Debug.Log("Item soltado");
    }

    private void IntentarRecuperarIngredientes()
    {
        if (!Input.GetKeyDown(teclaRecuperar)) return;
        if (zonaApuntada == null) return;
        if (inventario == null) return;

        zonaApuntada.RecuperarIngredientes(inventario, cuerpoJugador);
    }

    private int IdToTipoTe(string id)
    {
        switch (id)
        {
            case "TeVerde": return 0;
            case "TeBlanco": return 1;
            case "TeRojo": return 2;
            default: return -1;
        }
    }


}