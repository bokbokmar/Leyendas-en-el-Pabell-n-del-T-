using System.Collections.Generic;
using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    public Transform manoJugador;
    public int capacidadMaxima = 5;

    private List<ItemRecogible> items = new List<ItemRecogible>();

    public bool TieneEspacio() => items.Count < capacidadMaxima;
    public int CantidadItems() => items.Count;

    public bool AgregarItem(GameObject itemGO)
    {
        if (itemGO == null) return false;
        if (!TieneEspacio())
        {
            Destroy(itemGO);
            return false;
        }

        ItemRecogible item = itemGO.GetComponent<ItemRecogible>();
        if (item == null) item = itemGO.AddComponent<ItemRecogible>();

        items.Add(item);
        item.recogido = true;

        if (itemGO.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = true;
        if (itemGO.TryGetComponent<Collider>(out Collider col)) col.enabled = false;

        itemGO.transform.SetParent(manoJugador);
        itemGO.transform.localPosition = Vector3.zero;
        itemGO.transform.localRotation = Quaternion.identity;

        itemGO.transform.localScale = item.escalaEnMano;  

        return true;
    }
    public ItemRecogible SacarUltimoItem()
    {
        if (items.Count == 0) return null;

        ItemRecogible item = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);

        item.transform.SetParent(null);
        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb)) rb.isKinematic = false;
        if (item.TryGetComponent<Collider>(out Collider col)) col.enabled = true;
        item.recogido = false;
        return item;
    }
}