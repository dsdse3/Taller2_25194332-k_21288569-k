using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Administra el estado global de la partida, puntajes, UI, coleccionables y todo el sistema de audio.
/// Incluye soporte para activar el Menú de Derrota interactivo.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Text References")]
    public TextMeshProUGUI textoMonedas;
    public TextMeshProUGUI textoPuntaje;

    [Header("Menú de Derrota (Nuevo)")]
    // Aquí arrastraremos el objeto "MenuDerrota" desde la jerarquía
    public GameObject panelMenuDerrota; 

    [Header("Referencias de Juego")]
    public Transform jugadorTransform;
    private int cantidadMonedas = 0;
    private float posicionInicialZ;
    private bool juegoTerminado = false; // Control interno para congelar el puntaje al morir

    [Header("Asignación de Canales de Audio")]
    public AudioSource canalEfectos; 
    public AudioSource canalMusica;  

    [Header("Efectos de Sonido (AudioClips)")]
    public AudioClip sonidoDisparo;
    public AudioClip sonidoMoneda;
    public AudioClip sonidoMuerteEnemigo;

    [Header("Música del Juego")]
    public AudioClip musicaFondo;
    public AudioClip musicaDerrota;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (jugadorTransform != null)
        {
            posicionInicialZ = jugadorTransform.position.z;
        }
        
        // REQUISITO: Al iniciar la partida, el menú debe estar completamente oculto
        if (panelMenuDerrota != null)
        {
            panelMenuDerrota.SetActive(false);
        }

        juegoTerminado = false;
        ActualizarUI();

        // Configuramos la música de fondo de forma segura
        if (canalMusica != null && musicaFondo != null)
        {
            canalMusica.clip = musicaFondo;
            canalMusica.loop = true;
            canalMusica.playOnAwake = false;
            canalMusica.Play();
        }
    }

    void Update()
    {
        // Si el juego ya terminó por un choque, no seguimos aumentando el puntaje
        if (juegoTerminado) return;

        if (jugadorTransform != null)
        {
            float distanciaRecorridda = jugadorTransform.position.z - posicionInicialZ;
            int puntajeActual = Mathf.Max(0, Mathf.FloorToInt(distanciaRecorridda));
            textoPuntaje.text = "PUNTUACIÓN: " + puntajeActual.ToString("D8");
        }
    }

    // --- MÉTODOS DE CONTROL DE JUEGO & UI ---

    /// <summary>
    /// Hace aparecer el menú de derrota visual y cambia la música de fondo.
    /// </summary>
    public void MostrarMenuDerrota()
    {
        juegoTerminado = true;

        if (panelMenuDerrota != null)
        {
            panelMenuDerrota.SetActive(true); // Enciende el panel gris con el botón
        }

        // Ejecuta automáticamente el cambio de música a la pista de derrota
        ActivarMusicaDerrota();
    }

    /// <summary>
    /// Función pública que ejecutará tu botón "BotonReiniciar" al hacerle clic.
    /// </summary>
    public void EventoBotonReiniciar()
    {
        // Recarga la escena activa desde cero de forma segura
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    private void ActualizarUI()
    {
        textoMonedas.text = "MONEDAS: " + cantidadMonedas.ToString("D2");
    }

    // --- MÉTODOS DE AUDIO ---

    public void PlaySonidoDisparo()
    {
        if (sonidoDisparo != null && canalEfectos != null) 
            canalEfectos.PlayOneShot(sonidoDisparo);
    }

    public void SumarMoneda()
    {
        cantidadMonedas++;
        ActualizarUI();
        if (sonidoMoneda != null && canalEfectos != null) 
            canalEfectos.PlayOneShot(sonidoMoneda);
    }

    public void PlaySonidoMuerte()
    {
        if (sonidoMuerteEnemigo != null && canalEfectos != null) 
            canalEfectos.PlayOneShot(sonidoMuerteEnemigo);
    }

    public void ActivarMusicaDerrota()
    {
        if (canalMusica != null && musicaDerrota != null)
        {
            canalMusica.Stop(); 
            canalMusica.loop = false; 
            canalMusica.clip = musicaDerrota;
            canalMusica.Play(); 
        }
    }
}