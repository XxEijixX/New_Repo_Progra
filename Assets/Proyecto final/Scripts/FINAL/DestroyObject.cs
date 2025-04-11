using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public int objetosNecesarios = 8; // Número de objetos necesarios
    private InventoryHandler inventory; // Referencia al InventoryHandler
    [Header("Sonido")]
    public AudioClip destroySound; // Sonido de destrucción

    private void Start()
    {
        // Obtener la referencia al InventoryHandler
        inventory = FindObjectOfType<InventoryHandler>();

        // Asegurarnos de que el evento de agregar objetos sea monitorizado
        if (inventory == null)
        {
            Debug.LogError("InventoryHandler no encontrado.");
        }
    }

    private void Update()
    {
        // Comprobar si el jugador tiene los objetos necesarios
        if (inventory._Inventario.Count >= objetosNecesarios)
        {
            // Si ha recolectado los 8 objetos, destruir el objeto actual
            DestroyObjectWithSound();
        }
    }

    private void DestroyObjectWithSound()
    {
        // Reproducir el sonido de destrucción
        if (destroySound != null)
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
        }

        // Destruir el objeto
        Destroy(gameObject);
        Debug.Log("¡Los 8 objetos han sido recogidos! La puerta se destruye.");
    }
}
