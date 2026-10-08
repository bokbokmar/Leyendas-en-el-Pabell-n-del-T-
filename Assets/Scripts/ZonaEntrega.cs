using System.Collections.Generic;
using UnityEngine;

public class ZonaEntrega : MonoBehaviour
{
    [Header("Receta")]
    [Tooltip("IDs de los 2 ingredientes que acepta esta zona")]
    public string[] ingredientesRequeridos = new string[2];

    [Header("Resultado")]
    [Tooltip("Prefab que se genera al completar la receta")]
    public GameObject prefabResultado;

    [Tooltip("Dónde aparece el resultado. Si está vacío, aparece al lado de la zona")]
    public Transform puntoResultado;

    [Tooltip("Distancia a la que aparece el resultado si no hay puntoResultado")]
    public float offsetResultado = 1.5f;

    [Header("Feedback")]
    public GameObject efectoEntrega;
    public bool destruirIngredientes = true;

    [Header("Recuperar ingredientes")]
    [Tooltip("Si está marcado, el jugador puede sacar los ingredientes con una tecla")]
    public bool permitirRecuperar = true;

    [Tooltip("Dónde se devuelven los ingredientes al recuperarlos. Si está vacío, delante del jugador")]
    public Transform puntoDevolucion;

    private Dictionary<string, ItemRecogible> ingredientesColocados = new Dictionary<string, ItemRecogible>();

    public bool RecetaCompleta => ingredientesColocados.Count >= ingredientesRequeridos.Length;
    public int IngredientesColocados => ingredientesColocados.Count;
    public bool TieneIngredientes => ingredientesColocados.Count > 0;

  
    public bool Entregar(ItemRecogible item, InventarioJugador inventario)
    {
        if (item == null) return false;

        if (RecetaCompleta)
        {
            Debug.Log("ZonaEntrega: la receta ya está completa");
            return false;
        }

        string id = item.idIngrediente;

        bool esRequerido = false;
        foreach (string req in ingredientesRequeridos)
        {
            if (req == id) { esRequerido = true; break; }
        }

        if (!esRequerido)
        {
            Debug.Log($"ZonaEntrega: '{id}' no es un ingrediente válido aquí");
            return false;
        }

        if (ingredientesColocados.ContainsKey(id))
        {
            Debug.Log($"ZonaEntrega: ya tienes '{id}' colocado");
            return false;
        }

        ingredientesColocados[id] = item;

        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        if (item.TryGetComponent<Collider>(out Collider col))
            col.enabled = false;

        Debug.Log($"ZonaEntrega: ingrediente '{id}' colocado ({ingredientesColocados.Count}/{ingredientesRequeridos.Length})");

        if (RecetaCompleta)
            CompletarReceta();

        return true;
    }

    private void CompletarReceta()
    {
        Debug.Log("ZonaEntrega: ¡receta completa!");

        if (efectoEntrega != null)
            Instantiate(efectoEntrega, transform.position, Quaternion.identity);

        if (destruirIngredientes)
        {
            foreach (var kvp in ingredientesColocados)
            {
                if (kvp.Value != null)
                    Destroy(kvp.Value.gameObject);
            }
        }

        ingredientesColocados.Clear();

        if (prefabResultado != null)
        {
            Vector3 pos = (puntoResultado != null)
                ? puntoResultado.position
                : transform.position + transform.right * offsetResultado;

            Quaternion rot = (puntoResultado != null)
                ? puntoResultado.rotation
                : Quaternion.identity;

            Instantiate(prefabResultado, pos, rot);
            Debug.Log("ZonaEntrega: resultado generado");
        }
    }

    
    public bool RecuperarIngredientes(InventarioJugador inventario, Transform jugador)
    {
        if (!permitirRecuperar)
        {
            Debug.Log("ZonaEntrega: no se permite recuperar ingredientes");
            return false;
        }

        if (ingredientesColocados.Count == 0)
        {
            Debug.Log("ZonaEntrega: no hay ingredientes que recuperar");
            return false;
        }

        if (inventario == null)
        {
            Debug.Log("ZonaEntrega: no hay inventario");
            return false;
        }

        List<ItemRecogible> itemsADevolver = new List<ItemRecogible>(ingredientesColocados.Values);
        ingredientesColocados.Clear();

        foreach (ItemRecogible item in itemsADevolver)
        {
            if (item == null) continue;

            item.transform.SetParent(null);
            item.transform.localScale = item.escalaOriginal;   
            item.transform.localRotation = Quaternion.identity;

            bool ok = inventario.AgregarItem(item.gameObject);

            if (!ok)
            {
                Vector3 pos = (puntoDevolucion != null)
                    ? puntoDevolucion.position
                    : (jugador != null
                        ? jugador.position + jugador.forward * 1.5f
                        : transform.position + transform.right * offsetResultado);

                item.transform.position = pos;
                item.transform.rotation = Quaternion.identity;

                if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                if (item.TryGetComponent<Collider>(out Collider col))
                    col.enabled = true;
            }
        }

        Debug.Log($"ZonaEntrega: {itemsADevolver.Count} ingrediente(s) recuperado(s)");
        return true;
    }
}