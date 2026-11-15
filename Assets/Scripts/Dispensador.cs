using UnityEngine;

public class Dispensador : MonoBehaviour, IInteractuable
{
    [SerializeField] private InventarioJugador inventario;
    [SerializeField] private GameObject prefabADispensar;

    public void Interactuar()
    {
        if (inventario.objetoEnMano != null)
        {
            Debug.Log("Tienes las manos ocupadas");

        }
        else
        {
            Debug.Log("Interaccion");
            GameObject clon = Instantiate(prefabADispensar);
            inventario.AgregarItem(clon);
        }
    }
}