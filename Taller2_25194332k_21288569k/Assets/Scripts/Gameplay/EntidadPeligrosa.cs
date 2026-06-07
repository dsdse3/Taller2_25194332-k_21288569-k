using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar la pa

/// <summary>
/// Clase base (Padre) que define el comportamiento común de los obstáculos y enemigos.
/// Aplica conceptos de herencia para el Diagrama de Clases del taller.
/// </summary>
public class EntidadPeligrosa : MonoBehaviour
{
    // Ambos deben tener su respectivo Material asociado a su Modelo (se configura en el Inspector)
    [Header("Configuración Base")]
    public string nombreEntidad;

    // Detecta cuando la bala (que es un Trigger) impacta la entidad
    private void OnTriggerEnter(Collider other)
    {
        // Regla: Destruirse al recibir un proyectil (bala) del jugado
        if (other.CompareTag("Bala"))
        {
            // Aquí podrías activar un sonido de muerte si lo deseas
            EfectoAlMorir();
            Destroy(gameObject); // Se destruye el enemigo/obstáculo
        }
    }

    // Detecta cuando el jugador choca físicamente con el cuerpo del enemigo/obstáculo
    private void OnCollisionEnter(Collision collision)
    {
        // Regla: Matar al jugador al colisionar con él (reiniciando la partida)
        if (collision.gameObject.CompareTag("Player"))
        {
            ReiniciarPartida();
        }
    }

    /// <summary>
    /// Método virtual que puede ser modificado por los hijos (ej. sumar puntos o sonido específico).
    /// </summary>
    protected virtual void EfectoAlMorir()
    {
        Debug.Log(nombreEntidad + " ha sido destruido.");
    }

    private void ReiniciarPartida()
    {
        Debug.Log("El jugador ha muerto. Reiniciando partida...");
        // Recarga la escena actual desde cero, cumpliendo el requerimiento del taller
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}