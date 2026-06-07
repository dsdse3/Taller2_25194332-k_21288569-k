using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Administra la generación y destrucción dinámica de plataformas para crear un camino infinito.
/// También se encarga de poblar de manera aleatoria cada suelo con obstáculos, enemigos o monedas.
/// </summary>
public class SueloGenerator : MonoBehaviour
{
    [Header("Configuración de Plataformas")]
    public GameObject prefabSuelo; // El prefab del Plane 
    public int cantidadInicial = 12; // Cumple rango de 10 a 30 (Regla del taller)

    [Header("Prefabs de Elementos (Hijos)")]
    public GameObject[] prefabsObstaculos; // Arrastra aquí tus modelos de autos (mínimo 3)
    public GameObject prefabEnemigo;      // Arrastra aquí tu prefab de Zombie
    public GameObject prefabMoneda;       // Arrastra aquí tu prefab de Moneda

    [Header("Referencias")]
    public Transform jugadorTransform; // Para saber la posición Z del jugador

    // Configuración de carriles (Coherente con PlayerController: distancia de 3 metros)
    private float distanciaCarril = 3f; 
    private Queue<GameObject> suelosActivos = new Queue<GameObject>();
    private Vector3 proximaPosicionSpawn = Vector3.zero;
    private float largoSuelo = 10f; // Ajustar según el tamaño real de tu plano

    void Start()
    {
        // Al comenzar la partida se deben generar las plataformas iniciales requeridas
        for (int i = 0; i < cantidadInicial; i++)
        {
            // Las primeras 3 plataformas las dejamos vacías para que el jugador empiece seguro
            bool generarVacio = (i < 3);
            SpawnSuelo(generarVacio);
        }
    }

    void Update()
    {
        // Si el jugador ya avanzó lo suficiente y dejó atrás la plataforma más antigua
        if (suelosActivos.Count > 0 && jugadorTransform.position.z > (suelosActivos.Peek().transform.position.z + largoSuelo))
        {
            DestruirSueloViejo();
            SpawnSuelo(false); // Gatilla generar 1 suelo nuevo al final automáticamente
        }
    }

    /// <summary>
    /// Instancia una nueva plataforma al final del camino infinito y le asigna un elemento aleatorio.
    /// </summary>
    private void SpawnSuelo(bool vacio)
    {
        // Instancia el prefab del suelo en la posición calculada
        GameObject nuevoSuelo = Instantiate(prefabSuelo, proximaPosicionSpawn, Quaternion.identity);
        
        // Si no se requiere vacío, intentamos spawnear un obstáculo, enemigo o moneda
        if (!vacio)
        {
            GenerarElementoEnSuelo(nuevoSuelo);
        }

        // Lo añadimos a nuestra estructura de datos para su posterior eliminación
        suelosActivos.Enqueue(nuevoSuelo);

        // Calculamos la posición del siguiente suelo basándonos en el largo del actual
        proximaPosicionSpawn += Vector3.forward * largoSuelo;
    }

    /// <summary>
    /// Selecciona de forma aleatoria un carril y un objeto para instanciarlo como hijo de la plataforma.
    /// </summary>
    private void GenerarElementoEnSuelo(GameObject sueloPadre)
    {
        // 1. Determinar el carril de forma aleatoria (0 = Izquierda, 1 = Centro, 2 = Derecha)
        int carrilAleatorio = Random.Range(0, 3);
        float posX = (carrilAleatorio - 1) * distanciaCarril; // Da como resultado: -3, 0, o 3

        // La posición local relativa al centro del suelo actual
        // Ajustamos la altura (Y) según requieran tus modelos para que no queden flotando o enterrados
        Vector3 posicionLocal = new Vector3(posX, 0.5f, 0f); 

        // 2. Decidir qué tipo de elemento spawnear de forma aleatoria
        // 0 = Obstáculo (Auto), 1 = Enemigo (Zombie), 2 = Moneda, 3 = Suelo vacío (para dar respiro)
        int tipoElemento = Random.Range(0, 4);
        GameObject prefabElegido = null;

        switch (tipoElemento)
        {
            case 0:
                if (prefabsObstaculos != null && prefabsObstaculos.Length > 0)
                {
                    // Elige uno de los tipos de autos de manera aleatoria
                    prefabElegido = prefabsObstaculos[Random.Range(0, prefabsObstaculos.Length)];
                }
                break;
            case 1:
                prefabElegido = prefabEnemigo;
                break;
            case 2:
                prefabElegido = prefabMoneda;
                break;
            default:
                prefabElegido = null; // No spawnea nada en esta plataforma
                break;
        }

        // 3. Instanciar el objeto y emparentarlo en la jerarquía
        if (prefabElegido != null)
        {
            // Instanciamos usando la posición y rotación del suelo padre como referencia
            GameObject elementoInstanciado = Instantiate(prefabElegido, sueloPadre.transform.position + posicionLocal, prefabElegido.transform.rotation);
            
            // ¡ESTA ES LA REGLA CLAVE!: Se vuelve hijo de la plataforma
            elementoInstanciado.transform.SetParent(sueloPadre.transform);
        }
    }

    /// <summary>
    /// Elimina la plataforma más antigua que el jugador ya dejó atrás.
    /// </summary>
    private void DestruirSueloViejo()
    {
        GameObject sueloAEliminar = suelosActivos.Dequeue();
        // Destruye el GameObject (al destruir al padre, Unity elimina automáticamente a todos los hijos)
        Destroy(sueloAEliminar);
    }
}