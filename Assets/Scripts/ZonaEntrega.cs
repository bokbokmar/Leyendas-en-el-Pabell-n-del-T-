using UnityEngine;

public class ZonaEntrega : MonoBehaviour, IInteractuable
{
    public GameObject tazaTeVerde;
    public GameObject tazaTeBlanco;
    public GameObject tazaTeRojo;
    [SerializeField] private bool tieneAgua;
    [SerializeField] private bool tieneHojas;
    [SerializeField] private ItemRecogible hojasEnMaquina;
    [SerializeField] private InventarioJugador inventario;
    private ItemRecogible objetoAgregado;
    [SerializeField] private Transform puntoTaza;


    public void Interactuar()
    {
        if (inventario.objetoEnMano != null)
        {
            objetoAgregado = inventario.objetoEnMano.GetComponent<ItemRecogible>();
            Destroy(inventario.manoJugador.GetChild(0).gameObject);
            inventario.objetoEnMano = null;
        }
        else if (inventario.objetoEnMano == null && !tieneHojas && !tieneAgua)
        {
            Debug.Log("Primero trae algo");
            return;
        }

        if (tieneAgua && tieneHojas)
        {
            HacerTe();
        }
        else
        {
            switch (objetoAgregado.nombreItem)
            {
                case "HojaBlanca":
                    {
                        if (!tieneHojas)
                        {
                            Debug.Log("Te " + objetoAgregado.nombreItem + " agregado");
                            hojasEnMaquina = objetoAgregado;
                            objetoAgregado = null;
                            tieneHojas = true;
                        }
                        else
                        {
                            Debug.Log("Ya tiene " + hojasEnMaquina.nombreItem);
                        }

                        break;
                    }

                case "HojaRoja":
                    {
                        if (!tieneHojas)
                        {
                            Debug.Log("Te " + objetoAgregado.nombreItem + " agregado");
                            hojasEnMaquina = objetoAgregado;
                            objetoAgregado = null;
                            tieneHojas = true;
                        }
                        else
                        {
                            Debug.Log("Ya tiene " + hojasEnMaquina.nombreItem);
                        }

                        break;
                    }

                case "HojaVerde":
                    {
                        if (!tieneHojas)
                        {
                            Debug.Log("Te " + objetoAgregado.nombreItem + " agregado");
                            hojasEnMaquina = objetoAgregado;
                            objetoAgregado = null;
                            tieneHojas = true;
                        }
                        else
                        {
                            Debug.Log("Ya tiene " + hojasEnMaquina.nombreItem);
                        }

                        break;
                    }

                case "Agua":
                    {
                        if (!tieneAgua)
                        {
                            Debug.Log(objetoAgregado.nombreItem + " agregada");
                            objetoAgregado = null;
                            tieneAgua = true;
                        }
                        else
                        {
                            Debug.Log("Ya tiene " + hojasEnMaquina.nombreItem);
                        }

                        break;
                    }
            }
        }

    }

    public void HacerTe()
    {
        switch (hojasEnMaquina.nombreItem)
        {
            case "HojaBlanca":
                {
                    Instantiate(tazaTeBlanco, puntoTaza.transform.position, puntoTaza.transform.rotation);
                    break;
                }

            case "HojaVerde":
                {
                    Instantiate(tazaTeVerde, puntoTaza.transform.position, puntoTaza.transform.rotation);
                    break;
                }

            case "HojaRoja":
                {
                    Instantiate(tazaTeRojo, puntoTaza.transform.position, puntoTaza.transform.rotation);
                    break;
                }
        }

        hojasEnMaquina = null;
        objetoAgregado = null;
        tieneAgua = false;
        tieneHojas = false;
    }
}