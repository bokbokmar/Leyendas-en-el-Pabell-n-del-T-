using TMPro;
using UnityEngine;

public class Monedas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dineros;
    [SerializeField] private RelojInterfaz relojito;

    private void Start()
    {
        dineros = GameObject.Find("Dinerito").GetComponent<TextMeshProUGUI>();
        relojito = GameObject.Find("INTERFAZ").GetComponent<RelojInterfaz>();
    }

    public void Recoger()
    {
        relojito.dineros += 15;
        dineros.text = relojito.dineros.ToString();
        Destroy(gameObject);
    }
}