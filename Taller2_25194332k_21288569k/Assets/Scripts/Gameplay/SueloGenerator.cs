using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Administra la generación y destrucción dinámica de plataformas para crear un camino infinito.
/// </summary>
public class SueloGenerator : MonoBehaviour
{
    [Header("Configuración de Plataformas")]
    public GameObject prefabSuelo; // El prefab del Plane 
    public int cantidadInicial = 12; 

    [Header("Referencias")]
    public Transform jugadorTransform; // Para saber la posición Z del jugador

    // Estructura de datos de tipo Cola (Queue) para reciclar los suelos de manera ordenada (FIFO)
    private Queue<GameObject> suelosActivos = new Queue<GameObject>();
    private Vector3 proximaPosicionSpawn = Vector3.zero;
    private float largoSuelo = 10f; // Ajustar según el tamaño real de tu plano

    void Start()
    {
        // Al comenzar la partida se deben generar las plataformas iniciales requeridas
        for (int i = 0; i < cantidadInicial; i++)
        {
            // Las primeras plataformas pueden ser vacías para que el jugador empiece seguro
            SpawnSuelo();
        }
    }

    void Update()
    {
        // Si el jugador ya avanzó lo suficiente y dejó atrás la plataforma más antigua
        // Tomamos la posición Z del suelo más viejo en la cola y le sumamos su largo
        if (suelosActivos.Count > 0 && jugadorTransform.position.z > (suelosActivos.Peek().transform.position.z + largoSuelo))
        {
            DestruirSueloViejo();
            SpawnSuelo(); // Gatilla generar 1 suelo nuevo al final automáticamente
        }
    }

    /// <summary>
    /// Instancia una nueva plataforma al final del camino infinito.
    /// </summary>
    private void SpawnSuelo()
    {
        // Instancia el prefab en la posición calculada
        GameObject nuevoSuelo = Instantiate(prefabSuelo, proximaPosicionSpawn, Quaternion.identity);
        
        // Lo añadimos a nuestra estructura de datos para su posterior eliminación
        suelosActivos.Enqueue(nuevoSuelo);

        // Calculamos la posición del siguiente suelo basándonos en el largo del actual
        proximaPosicionSpawn += Vector3.forward * largoSuelo;
    }

    /// <summary>
    /// Elimina la plataforma más antigua que el jugador ya dejó atrás.
    /// </summary>
    private void DestruirSueloViejo()
    {
        // Saca el suelo más antiguo de la cola de ejecución
        GameObject sueloAEliminar = suelosActivos.Dequeue();
        
        // Destruye el GameObject de la escena (lo que también eliminará a sus hijos automáticamente)
        Destroy(sueloAEliminar);
    }
}