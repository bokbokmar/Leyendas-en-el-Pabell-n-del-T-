using System.Collections.Generic;
using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    public Transform manoJugador;
    public int capacidadMaxima;

    [SerializeField] private List<ItemRecogible> items = new List<ItemRecogible>();
    public GameObject objetoEnMano;

    private void Update()
    {
        TirarObjetoEnMano();
    }

    public bool TieneEspacio() => items.Count < capacidadMaxima;
    public int CantidadItems() => items.Count;

    public void AgregarItem(GameObject objetito)
    {
        if (objetoEnMano != null) { return; }

        objetoEnMano = objetito;
        objetoEnMano.transform.SetParent(manoJugador);
        objetoEnMano.transform.position = manoJugador.position;
        objetoEnMano.transform.rotation = manoJugador.rotation;
        objetoEnMano.GetComponent<Rigidbody>().useGravity = false;
        objetoEnMano.GetComponent<Rigidbody>().isKinematic = true;
        //GameObject clon = Instantiate(objetoEnMano, manoJugador.transform.position, manoJugador.transform.rotation, manoJugador);
        //clon.GetComponent<Rigidbody>().useGravity = false;
    }

    public void TirarObjetoEnMano()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (manoJugador.childCount < 1 || objetoEnMano == null)
            {
                Debug.Log("No traes nada en la mano :)");
                return;
            }
            //Vector3 posicion = manoJugador.transform.position + transform.forward * 15f;
            //posicion.y = 0; //Este vector es para dejar el objeto de la mano en el suelo

            GameObject objetoTirado = manoJugador.GetChild(0).gameObject;

            //objetoTirado.transform.position = posicion;
            //objetoTirado.transform.rotation = Quaternion.identity;
            objetoTirado.transform.SetParent(null);
            objetoTirado.GetComponent<Rigidbody>().useGravity = true;
            objetoEnMano.GetComponent<Rigidbody>().isKinematic = false;
            objetoEnMano = null;
        }
    }
}