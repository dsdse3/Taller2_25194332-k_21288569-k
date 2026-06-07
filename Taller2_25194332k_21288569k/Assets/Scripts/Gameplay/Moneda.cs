using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla la recolección de monedas por parte del jugador y actualiza el contador global.
/// </summary>
public class Moneda : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Regla del taller: Detectar si el objeto que la atraviesa es el Jugador 
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Moneda recolectada por el jugador!");

            // intentamos sumarla al contador global a través del GameManager 
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SumarMoneda();
            }

            // la moneda desaparece de la escena inmediatamente 
            Destroy(gameObject);
        }
    }
}