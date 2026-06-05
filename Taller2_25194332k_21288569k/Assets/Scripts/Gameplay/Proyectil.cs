using UnityEngine;

/// <summary>
/// Controla el desplazamiento lineal del proyectil y detecta impactos con obstáculos o enemigos.
/// </summary>
public class Proyectil : MonoBehaviour
{
    [Header("Configuracion")]
    public float velocidadBala = 25f;
    public float tiempoDeVida = 3f;

    void Start()
    {
        Destroy(gameObject, tiempoDeVida);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * velocidadBala * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo") || other.CompareTag("Obstaculo"))
        {
            Destroy(gameObject);
        }
    }
}