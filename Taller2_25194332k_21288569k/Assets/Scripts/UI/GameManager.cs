using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necesario para usarTextMeshPro (el sistema moderno de texto de Unity)


// Clase que administra el estado global de la partida, puntajes, UI y coleccionables.
public class GameManager : MonoBehaviour
{
    // Instancia estática del Singleton
    public static GameManager Instance { get; private set; }

    [Header("UI Text References")]
    // Referencias a los textos de la UI para mostrar monedas y puntaje
    public TextMeshProUGUI textoMonedas;
    // El texto de puntaje se actualizará dinámicamente según la distancia recorrida por el jugador
    public TextMeshProUGUI textoPuntaje;

    [Header("Referencias de Juego")]
    // Referencia al transform del jugador para calcular la distancia recorrida
    public Transform jugadorTransform;
    // Contador de monedas recolectadas por el jugador durante la partida
    private int cantidadMonedas = 0;
    // Guardamos la posición inicial en Z del jugador para calcular la distancia real recorrida
    private float posicionInicialZ;

    void Awake()
    {
        // Configuración del Singleton seguro
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Guardamos el punto de inicio en Z del jugador para calcular la distancia real recorrida
        if (jugadorTransform != null)
        {
            posicionInicialZ = jugadorTransform.position.z;
        }
        
        ActualizarUI();
    }

    void Update()
    {
        // El puntaje incrementa según la distancia que va avanzando el jugador (Regla del taller)
        if (jugadorTransform != null)
        {
            float distanciaRecorridda = jugadorTransform.position.z - posicionInicialZ;
            int puntajeActual = Mathf.Max(0, Mathf.FloorToInt(distanciaRecorridda));
            
            // Mostramos el puntaje con formato de ocho dígitos (ej: 00001234) coincidiendo con la rúbrica
            textoPuntaje.text = "PUNTUACIÓN: " + puntajeActual.ToString("D8");
        }
    }

 
    // Incrementa el contador de monedas y refresca el texto en la pantalla.
    public void SumarMoneda()
    {
        cantidadMonedas++;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        // Muestra las monedas con formato de dos dígitos (ej: 09) coincidiendo con la rúbrica
        textoMonedas.text = "MONEDAS: " + cantidadMonedas.ToString("D2");
    }
}