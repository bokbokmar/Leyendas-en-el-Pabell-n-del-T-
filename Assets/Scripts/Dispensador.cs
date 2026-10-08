using System.Collections.Generic;
using UnityEngine;

public class Dispensador : MonoBehaviour
{
    public List<GameObject> prefabsItems;  

    public GameObject EntregarItem()
    {
        if (prefabsItems.Count == 0) return null;

        GameObject prefab = prefabsItems[Random.Range(0, prefabsItems.Count)];
        return Instantiate(prefab); 
    }
}