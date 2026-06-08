using UnityEngine;
using System.Collections; // Necesario para usar Corrutinas (IEnumerator)

/// <summary>
/// Controla el movimiento hacia adelante, el cambio de carriles, el salto, el disparo del jugador
/// y gatilla la secuencia de audio/espera al perder la partida.
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

    // Control de seguridad interno para que la muerte no se ejecute muchas veces seguidas
    private bool estaMuerto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        estaMuerto = false; // Al comenzar, el jugador está vivo
    }

    void Update()
    {
        // Si el jugador ya colisionó contra un peligro, bloqueamos los controles y el avance
        if (estaMuerto) return;

        // Movimiento constante hacia adelante
        transform.Translate(Vector3.forward * velocidadAvance * Time.deltaTime);

        // Control de cambio de carriles
        if (Input.GetKeyDown(KeyCode.D)) CambiarCarril(1);
        if (Input.GetKeyDown(KeyCode.A)) CambiarCarril(-1);

        Vector3 posicionDeseada = transform.position;
        posicionDeseada.x = (carrilActual - 1) * distanciaCarril;
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, 10f * Time.deltaTime);

        // Mecánica de Salto
        if (Input.GetKeyDown(KeyCode.Space) && enElSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            enElSuelo = false;
        }

        // Mecánica de Disparo con retroalimentación de audio
        if (Input.GetKeyDown(KeyCode.F) && Time.time >= proximoDisparo)
        {
            DispararProyectil();
            proximoDisparo = Time.time + tiempoEntreDisparos;

            // REGLA DEL TALLER: Llama al GameManager para reproducir el audio del disparo
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PlaySonidoDisparo();
            }
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
        // Si toca el piso firme, le permite volver a saltar
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enElSuelo = true;
        }
        
        // MEDIDA DE SEGURIDAD CON AUDIO: Si choca físicamente contra un Obstáculo o un Enemigo,
        // gatillamos la secuencia con retraso para escuchar la cortina musical de derrota.
        if (!estaMuerto && (collision.gameObject.CompareTag("Obstaculo") || collision.gameObject.CompareTag("Enemigo")))
        {
            StartCoroutine(SecuenciaMuerte());
        }
    }

    /// <summary>
    /// Frena por completo las físicas y velocidad del personaje, activa la música de derrota
    /// y aguarda un instante prudente antes de recargar el nivel.
    /// </summary>
    private IEnumerator SecuenciaMuerte()
    {
        estaMuerto = true;
        Debug.Log("Impacto físico detectado con peligro. Reproduciendo música de Game Over...");
        
        // 1. Frenamos en seco la velocidad de avance para que no siga corriendo por el mapa
        velocidadAvance = 0f;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;// Resetea cualquier inercia física acumulada
        }

        // 2. Activamos la pista musical de derrota desde nuestro Singleton centralizado
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ActivarMusicaDerrota();
        }

        // 3. Dejamos que la música suene por 2.5 segundos antes de limpiar la escena
        yield return new WaitForSeconds(2.5f);

        // 4. Reiniciamos la partida cargando la escena actual
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}