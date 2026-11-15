using UnityEngine;

public class ItemRecogible : MonoBehaviour, IInteractuable
{
    public string nombreItem;
    public int idIngrediente;
    public Sprite icono;
    public Vector3 escalaEnMano = new Vector3(0.02f, 0.02f, 0.02f);
    [HideInInspector] public Vector3 escalaOriginal;
    public InventarioJugador inventario;

    public void Interactuar()
    {
        Recoger();
    }

    public void Recoger()
    {
        Debug.Log("Taza");
        //gameObject.transform.position = new Vector3(0, 0, 0);
        inventario.AgregarItem(this.gameObject);
        //Destroy(gameObject);
    }
}