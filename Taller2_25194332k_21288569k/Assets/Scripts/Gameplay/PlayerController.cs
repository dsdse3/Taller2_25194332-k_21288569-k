using UnityEngine;

/// <summary>
/// Controla el movimiento hacia adelante, el cambio de carriles, el salto y el disparo del jugador.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadAvance = 10f;
    public float distanciaCarril = 3f;
    private int carrilActual = 1;

    [Header("Salto")]
    public float fuerzaSalto = 7f;
    private Rigidbody rb;
    private bool enElSuelo = true;

    [Header("Ataque")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo;
    public float tiempoEntreDisparos = 0.5f;
    private float proximoDisparo = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidadAvance * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.D)) CambiarCarril(1);
        if (Input.GetKeyDown(KeyCode.A)) CambiarCarril(-1);

        Vector3 posicionDeseada = transform.position;
        posicionDeseada.x = (carrilActual - 1) * distanciaCarril;
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, 10f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && enElSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            enElSuelo = false;
        }

        if (Input.GetKeyDown(KeyCode.F) && Time.time >= proximoDisparo)
        {
            DispararProyectil();
            proximoDisparo = Time.time + tiempoEntreDisparos;
        }
    }

    private void DispararProyectil()
    {
        Vector3 posicionOrigen = puntoDisparo != null ? puntoDisparo.position : transform.position + transform.forward;
        Quaternion rotacionOrigen = puntoDisparo != null ? puntoDisparo.rotation : transform.rotation;
        Instantiate(prefabProyectil, posicionOrigen, rotacionOrigen);
    }

    private void CambiarCarril(int direccion)
    {
        carrilActual += direccion;
        carrilActual = Mathf.Clamp(carrilActual, 0, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enElSuelo = true;
        }
    }
}