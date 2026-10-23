
using UnityEngine;

public class SalidaClientes : MonoBehaviour
{
    private Spawner spawner;

    private void Start()
    {
        spawner = transform.parent.GetComponent<Spawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cliente"))
        {
            Clientes cliente = other.GetComponent<Clientes>();
            if (cliente != null && (cliente.clienteInsatisfecho || cliente.clienteSatisfecho))
            {
                spawner.asientoTomado[cliente.auxiliarsoide] = false;
                Destroy(other.gameObject);
            }
        }
    }
}