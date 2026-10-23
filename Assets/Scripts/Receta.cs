using System;
using UnityEngine;

[Serializable]
public class Receta
{
    [Tooltip("IDs de los ingredientes necesarios (ej. 'HojaVerde', 'Agua')")]
    public string[] ingredientes;

    [Tooltip("Prefab del resultado (ej. TeVerde)")]
    public GameObject resultado;

    [Tooltip("Nombre legible de la receta (para debug)")]
    public string nombre = "Receta";
}