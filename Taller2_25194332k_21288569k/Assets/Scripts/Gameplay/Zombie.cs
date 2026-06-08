using UnityEngine;

/// <summary>
/// Controla el comportamiento específico del Zombie y gatilla su audio de muerte de forma segura.
/// </summary>
public class Zombie : EntidadPeligrosa
{
    [Header("Movimiento del Zombie")]
    public float velocidadZombie = 4f;

    [Header("Audio Local")]
    // Arrastra aquí el sonido del zombie directo en el prefab
    public AudioClip sonidoMuerte; 

    void Update()
    {
        transform.Translate(Vector3.back * velocidadZombie * Time.deltaTime, Space.World);
    }

    // Sobreescribimos el método de muerte
    protected override void EfectoAlMorir()
    {
        // Ejecuta lo que tenga la clase padre (EntidadPeligrosa)
        base.EfectoAlMorir(); 

        // REGLA DE ORO: Si hay un sonido asignado, lo creamos en el espacio 3D
        // para que siga sonando aunque el objeto zombie sea destruido inmediatamente.
        if (sonidoMuerte != null)
        {
            AudioSource.PlayClipAtPoint(sonidoMuerte, transform.position, 1.0f);
        }
    }
}