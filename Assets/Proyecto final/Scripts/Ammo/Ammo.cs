using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int balasAAgregar = 5; // Cantidad de balas que se agregarán al jugador
    [SerializeField] private AudioClip sonidoDestruccion; // Sonido a reproducir al destruirse el objeto

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que colisiona tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Accede al script del jugador (RayCostco) y recarga balas
            other.transform.GetChild(0).GetChild(0).GetComponent<RayCostco>().Recargar(balasAAgregar);

            // Destruye el objeto recolector de balas
            DestruirConSonido();
        }
    }

    private void DestruirConSonido()
    {
        // Reproduce el sonido de destrucción si está configurado
        if (sonidoDestruccion != null)
        {
            AudioSource.PlayClipAtPoint(sonidoDestruccion, transform.position);
        }

        // Destruye el objeto recolector de balas
        Destroy(gameObject);
    }
}
