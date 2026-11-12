using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    [SerializeField] private float rangoInteraccion;
    [SerializeField] private InventarioJugador inventario;
    [SerializeField] private Transform camara;
    [SerializeField] private ZonaEntrega zona;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IntentarInteractuar();
        }
        IntentarSoltar();
        IntentarRecuperarIngredientes();
    }

    private void IntentarInteractuar()
    {
        Ray rayo = new Ray(camara.position, camara.forward);
        Debug.DrawRay(camara.position, camara.forward * rangoInteraccion, Color.red);

        if (Physics.Raycast(rayo, out RaycastHit hit, rangoInteraccion))
        {
            if (hit.collider.TryGetComponent(out IInteractuable objetoInteraccion))
            {
                objetoInteraccion.Interactuar();
            }
        }
    }

    private void IntentarSoltar()
    {
        if (Input.GetKeyDown(KeyCode.Q) && inventario.CantidadItems() != 0)
        {
            ItemRecogible itemEnMano = inventario.SacarUltimoItem();
            if (itemEnMano == null) { return; }

            Vector3 posicion = transform.parent.transform.position + transform.forward * 1.5f;
            posicion.y = 0; //Este vector es para dejar el objeto de la mano en el suelo
            itemEnMano.transform.position = posicion;
            itemEnMano.transform.rotation = Quaternion.identity;
            itemEnMano.transform.SetParent(null);

            //if (itemEnMano.TryGetComponent<Rigidbody>(out Rigidbody rb))
            //{
            //    rb.isKinematic = false;
            //    rb.useGravity = true;
            //    rb.linearVelocity = Vector3.zero;
            //    rb.angularVelocity = Vector3.zero;
            //}

            if (itemEnMano.TryGetComponent<Collider>(out Collider col)) { col.enabled = true; }

            Debug.Log("Item soltado");
        }
    }

    private void IntentarRecuperarIngredientes()
    {
        Ray rayo = new Ray(transform.position, transform.forward);
        Debug.DrawRay(camara.position, camara.forward * rangoInteraccion, Color.red);

        //if (Physics.Raycast(rayo, out RaycastHit hit, rangoInteraccion))
        //{
        //    if (hit.collider.TryGetComponent(out IInteractuable objetoInteraccion))
        //    {
        //        objetoInteraccion.Interactuar();
        //    }
        //}

        if (Physics.Raycast(rayo, out RaycastHit rec, rangoInteraccion) && Input.GetKeyDown(KeyCode.R))
        {
            zona.RecuperarIngredientes(inventario, transform);
        }
    }
}
