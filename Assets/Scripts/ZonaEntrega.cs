using System.Collections.Generic;
using UnityEngine;

public class ZonaEntrega : MonoBehaviour
{
    [Header("Recetas disponibles")]
    [Tooltip("Cada receta tiene sus ingredientes y su resultado")]
    public List<Receta> recetas = new List<Receta>();

    [Header("Resultado")]
    [Tooltip("Dónde aparece el resultado. Si está vacío, aparece al lado de la zona")]
    public Transform puntoResultado;

    [Tooltip("Distancia a la que aparece el resultado si no hay puntoResultado")]
    public float offsetResultado = 1.5f;

    [Header("Feedback")]
    public GameObject efectoEntrega;
    public bool destruirIngredientes = true;

    [Header("Recuperar ingredientes")]
    public bool permitirRecuperar = true;
    public Transform puntoDevolucion;

    private Dictionary<string, ItemRecogible> ingredientesColocados = new Dictionary<string, ItemRecogible>();

    public bool TieneIngredientes => ingredientesColocados.Count > 0;
    public int IngredientesColocados => ingredientesColocados.Count;

    public bool Entregar(ItemRecogible item, InventarioJugador inventario)
    {
        if (item == null) return false;

        string id = item.idIngrediente;

        if (ingredientesColocados.ContainsKey(id))
        {
            Debug.Log($"ZonaEntrega: ya tienes '{id}' colocado");
            return false;
        }

        bool esValido = false;
        foreach (Receta r in recetas)
        {
            foreach (string ing in r.ingredientes)
            {
                if (ing == id) { esValido = true; break; }
            }
            if (esValido) break;
        }

        if (!esValido)
        {
            Debug.Log($"ZonaEntrega: '{id}' no es ingrediente de ninguna receta");
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

        Debug.Log($"ZonaEntrega: '{id}' colocado ({ingredientesColocados.Count})");

        Receta recetaCompleta = BuscarRecetaCompleta();
        if (recetaCompleta != null)
            CompletarReceta(recetaCompleta);

        return true;
    }

    private Receta BuscarRecetaCompleta()
    {
        foreach (Receta r in recetas)
        {
            bool completa = true;
            foreach (string ing in r.ingredientes)
            {
                if (!ingredientesColocados.ContainsKey(ing))
                {
                    completa = false;
                    break;
                }
            }
            if (completa) return r;
        }
        return null;
    }

    private void CompletarReceta(Receta receta)
    {
        Debug.Log($"ZonaEntrega: ¡receta completa! → {receta.nombre}");

        if (efectoEntrega != null)
            Instantiate(efectoEntrega, transform.position, Quaternion.identity);

        if (destruirIngredientes)
        {
            foreach (string ing in receta.ingredientes)
            {
                if (ingredientesColocados.ContainsKey(ing))
                {
                    ItemRecogible item = ingredientesColocados[ing];
                    if (item != null) Destroy(item.gameObject);
                    ingredientesColocados.Remove(ing);
                }
            }
        }
        else
        {
            foreach (string ing in receta.ingredientes)
            {
                if (ingredientesColocados.ContainsKey(ing))
                {
                    ItemRecogible item = ingredientesColocados[ing];
                    if (item != null) item.transform.SetParent(null);
                    ingredientesColocados.Remove(ing);
                }
            }
        }

        if (receta.resultado != null)
        {
            Vector3 pos = (puntoResultado != null)
                ? puntoResultado.position
                : transform.position + transform.right * offsetResultado;

            pos.y += 0.3f;

            GameObject resultado = Instantiate(receta.resultado, pos, Quaternion.identity);

            if (resultado.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }

            StartCoroutine(ActivarFisicaTrasDelay(resultado, 0.2f));
        }
    }

    private System.Collections.IEnumerator ActivarFisicaTrasDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj == null) yield break;

        if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        if (obj.TryGetComponent<Collider>(out Collider col))
            col.enabled = true;
    }

    public bool RecuperarIngredientes(InventarioJugador inventario, Transform jugador)
    {
        if (!permitirRecuperar) return false;
        if (ingredientesColocados.Count == 0) return false;
        if (inventario == null) return false;

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

                if (item.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
                if (item.TryGetComponent<Collider>(out Collider col))
                    col.enabled = true;
            }
        }

        Debug.Log($"ZonaEntrega: {itemsADevolver.Count} ingrediente(s) recuperado(s)");
        return true;
    }
}