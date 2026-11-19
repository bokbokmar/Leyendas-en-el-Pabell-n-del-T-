using TMPro;
using UnityEngine;

public class Monedas : MonoBehaviour, IInteractuable
{
    public GameManager manager;
    private void Start()
    {
        //dineros = GameObject.Find("Dinerito").GetComponent<TextMeshProUGUI>();
        //relojito = GameObject.Find("INTERFAZ").GetComponent<RelojInterfaz>();
    }

    public void Interactuar()
    {
        Recoger();
    }

    public void Recoger()
    {
        manager.dinerito += 15;
        manager.ActualizarDinero();
        Destroy(gameObject);
    }
}