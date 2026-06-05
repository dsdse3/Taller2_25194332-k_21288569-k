using UnityEngine;

// Todo Script debe estar documentado debidamente
/// <summary>
/// Controla el movimiento hacia adelante, el cambio de carriles y el salto del jugador.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadAvance = 10f; // Velocidad forzada hacia adelante
    public float distanciaCarril = 3f; // Distancia entre cada uno de los 3 carriles
    private int carrilActual = 1; // 0: Izquierda, 1: Centro, 2: Derecha

    [Header("Configuración de Salto")]
    public float fuerzaSalto = 7f;
    private Rigidbody rb;
    private bool enElSuelo = true; // Variable para evitar saltar en el aire

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. Movimiento hacia adelante forzado sin detenerse
        transform.Translate(Vector3.forward * velocidadAvance * Time.deltaTime);

        // 2. Moverse en 2 direcciones horizontales con teclas A y D
        if (Input.GetKeyDown(KeyCode.D))
        {
            CambiarCarril(1); // Mover a la derecha
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            CambiarCarril(-1); // Mover a la izquierda
        }

        // Calcular la posición X deseada según el carril actual
        Vector3 posicionDeseada = transform.position;
        // Si el carril es 1 (centro), la posición X será 0. 
        posicionDeseada.x = (carrilActual - 1) * distanciaCarril; 
        
        // Mover al jugador suavemente hacia el carril deseado usando Lerp
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, 10f * Time.deltaTime);

        // 3. Saltar hacia arriba
        if (Input.GetKeyDown(KeyCode.Space) && enElSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            enElSuelo = false;
        }
    }

    /// <summary>
    /// Actualiza el índice del carril asegurando que se mantenga entre 0 y 2.
    /// </summary>
    private void CambiarCarril(int direccion)
    {
        carrilActual += direccion;
        carrilActual = Mathf.Clamp(carrilActual, 0, 2); // Limita el valor entre el carril 0 y el 2
    }

    // Detectar colisión con el suelo para poder volver a saltar
    private void OnCollisionEnter(Collision collision)
    {
        // Asegúrate de crear un Tag llamado "Suelo" y ponérselo a tus plataformas
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enElSuelo = true;
        }
    }
}