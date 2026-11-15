using UnityEngine;

public class ControladorJugador : MonoBehaviour
{
    [SerializeField] private float rangoInteraccion;
    [SerializeField] private InventarioJugador inventario;
    [SerializeField] private Transform camara;
    [SerializeField] private Maquina zona;

    private void Update()
    {
        Debug.DrawRay(camara.position, camara.forward * rangoInteraccion, Color.red);
        if (Input.GetKeyDown(KeyCode.E))
        {
            IntentarInteractuar();
        }
    }

    private void IntentarInteractuar()
    {
        Ray rayo = new Ray(camara.position, camara.forward);

        if (Physics.Raycast(rayo, out RaycastHit hit, rangoInteraccion))
        {
            if (hit.collider.TryGetComponent(out IInteractuable objetoInteraccion))
            {
                objetoInteraccion.Interactuar();
            }
        }
    }
}
