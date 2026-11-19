using UnityEngine;

public class CamaraJugador : MonoBehaviour
{
    [Header("Movimiento")]
    public float sensibilidad = 3f;
    public Transform cuerpoJugador;

    [Header("Interacción")]
    public int distancia = 4;
    public LayerMask ignorar;
    public float distanciaMaximaInteraccion = 3f;
    public KeyCode teclaInteractuar = KeyCode.E;
    public KeyCode teclaSoltar = KeyCode.Q;
    public KeyCode teclaRecuperar = KeyCode.R;

    private float rotacionX = 0f;
    private float rotacionY = 0f;
    [SerializeField] private InventarioJugador inventario;



    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventario = GetComponentInParent<InventarioJugador>();
    }

    private void Update()
    {
        MoverCamara();
    }

    private void MoverCamara()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -80f, 80f);
        rotacionY += mouseX;

        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);
        cuerpoJugador.localRotation = Quaternion.Euler(0f, rotacionY, 0f);
    }
}