using UnityEngine;

public class ItemRecogible : MonoBehaviour, IInteractuable
{
    public string nombreItem;
    public string idIngrediente;
    public Sprite icono;
    public Vector3 escalaEnMano = new Vector3(0.2f, 0.2f, 0.2f);

    [HideInInspector] public bool recogido = false;
    [HideInInspector] public Vector3 escalaOriginal;

    public void Interactuar()
    {

    }
}