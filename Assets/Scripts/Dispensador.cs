using UnityEngine;

public class Dispensador : MonoBehaviour, IInteractuable
{
    [SerializeField] private InventarioJugador inventario;
    [SerializeField] private GameObject prefabADispensar;

    //public GameObject EntregarItem()
    //{
    //    if (prefabsItems.Count == 0) return null;

    //    GameObject prefab = prefabsItems[Random.Range(0, prefabsItems.Count)];
    //    return Instantiate(prefab);
    //}

    public void Interactuar()
    {
        Debug.Log("Interaccion");
        inventario.AgregarItem(prefabADispensar);
        //GameObject clon = Instantiate(prefabADispensar, inventario.manoJugador);
        //inventario.AgregarItem(clon);
    }
}