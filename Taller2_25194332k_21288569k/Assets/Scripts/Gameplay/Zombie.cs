using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Esta Clase controla el comportamiento específico del Zombie, moviendose hacia el jugador en línea recta.
// Hereda las propiedades de colisión y muerte de EntidadPeligrosa.

public class Zombie : EntidadPeligrosa
{
    [Header("Movimiento del Zombie")]
    // Velocidad a la que el zombie se mueve hacia el jugador (ajustable desde el Inspector)
    public float velocidadZombie = 4f;

    void Update()
    {
        // Esto ignora la rotación interna del modelo y garantiza que vaya EN CONTRA del jugador.
        transform.Translate(Vector3.back * velocidadZombie * Time.deltaTime, Space.World);
    }

    // Opcional: Puedes sobreescribir el método virtual si quieres añadir efectos específicos
    protected override void EfectoAlMorir()
    {
        base.EfectoAlMorir();
        // Aquí irá el código para activar el sonido de muerte del enemigo solicitado en la entrega
    }
}
