using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField] public List<Item> inventario; // Lista del inventario
    public List<Item> _Inventario { get => inventario; } // Lectura segura desde otros scripts

    public int maxCapacity = 24; // Capacidad máxima del inventario

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            TirarObjeto();
        }
    }

    public void AgregarObjeto(Item item)
    {
        if (inventario.Count < maxCapacity)
        {
            inventario.Add(item);
        }
        else
        {
            Debug.LogWarning("Inventario lleno, no se puede agregar más objetos.");
        }
    }

    public void TirarObjeto()
    {
        if (inventario.Count == 0)
        {
            Debug.LogWarning("No hay objetos en el inventario para tirar.");
            return;
        }

        // Siempre tirar el primer objeto en la lista (orden FIFO)
        Item itemATirar = inventario[0];

        // Buscar el prefab del objeto en el inventario y reactivarlo
        GameObject objeto = itemATirar._prefab;

        // Asegurarse de que el objeto se cree en la escena
        GameObject objetoInstanciado = Instantiate(objeto, transform.position, transform.rotation);

        // Asegúrate de que el objeto esté visible (activar los renderers)
        Renderer[] renderers = objetoInstanciado.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.enabled = true; // Asegurarse de que el renderer esté habilitado
        }

        // Si el objeto tiene un Collider, asegúrate de activarlo también
        Collider[] colliders = objetoInstanciado.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        // Eliminarlo del inventario
        inventario.RemoveAt(0);
    }
}
