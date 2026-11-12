using UnityEngine;

public class MoviemientoJugador : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float movX;
    [SerializeField] private float movZ;
    [SerializeField] private float velocidad = 5f;

    [Header("Jetpack")]
    public float fuerzaJetpack = 8f;
    public float combustibleMaximo = 3f;
    public float consumoPorSegundo = 1f;
    public float recargaPorSegundo = 1.5f;
    public KeyCode teclaJetpack = KeyCode.Space;

    [Header("Gravedad")]
    public float gravedad = -9.81f;

    private CharacterController controlador;
    private float combustibleActual;
    private float velocidadVertical = 0f;
    private bool enSuelo;

    public float CombustibleActual => combustibleActual;
    public float CombustibleMaximo => combustibleMaximo;

    private void Start()
    {
        controlador = GetComponent<CharacterController>();
        combustibleActual = combustibleMaximo;
    }

    private void Update()
    {
        enSuelo = controlador.isGrounded;

        Moverse();
        AplicarGravedad();
        ProcesarJetpack();
        RecargarSiTocaSuelo();

        Vector3 movimientoFinal = (transform.right * movX + transform.forward * movZ) * velocidad;
        movimientoFinal.y = velocidadVertical;

        controlador.Move(movimientoFinal * Time.deltaTime);
    }

    private void Moverse()
    {
        movX = Input.GetAxis("Horizontal");
        movZ = Input.GetAxis("Vertical");
    }

    private void AplicarGravedad()
    {
        if (enSuelo && velocidadVertical < 0f)
            velocidadVertical = -2f;
        else
            velocidadVertical += gravedad * Time.deltaTime;
    }

    private void ProcesarJetpack()
    {
        bool quiereVolar = Input.GetKey(teclaJetpack);
        bool tieneCombustible = combustibleActual > 0f;

        if (quiereVolar && tieneCombustible)
        {
            velocidadVertical = fuerzaJetpack;
            combustibleActual -= consumoPorSegundo * Time.deltaTime;
            combustibleActual = Mathf.Max(combustibleActual, 0f);
        }
    }

    private void RecargarSiTocaSuelo()
    {
        if (enSuelo && combustibleActual < combustibleMaximo)
        {
            combustibleActual += recargaPorSegundo * Time.deltaTime;
            combustibleActual = Mathf.Min(combustibleActual, combustibleMaximo);
        }
    }
}