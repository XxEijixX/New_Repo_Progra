using UnityEngine;

public class InventoryUIHandler : MonoBehaviour
{

    [SerializeField] private GameObject inventoryPanel; // El objeto de la hierarchy que contiene el UI de el Inventario
    [SerializeField] private GameObject uiItem; // El prefab de los objetos que se mostraran en el inventario. Contiene Imagen, Nombre y Descripcion del objeto
    [SerializeField] private GameObject instanceDestination; // En donde se van a instanciar los items, para poderlos emparentar y que se acomoden segun el Layout Group
    private GameObject[] itemsInstanciados = new GameObject[24]; // Aqui guardo los items instanciados para despues usarlos por pagina, que si del 0 al 7, del 8 al 15 y asi sucesivamente

    private InventoryHandler inventario; // Referencia al inventario
    private bool inventoryOpened = false; // Si tengo o no abierto el inventario

    private int actualPage = 0;

    private void Start()
    {
        // Consigo referencias
        inventario = FindObjectOfType<InventoryHandler>();
        itemsInstanciados = new GameObject[inventario.maxCapacity]; // Asigno el tamaño del arreglo a mi capacidad maxima de items
    }

    private void Update()
    {
        ToggleInventory();

        // --- NUEVO: Cambiar de página con flechas del teclado ---
        if (inventoryOpened)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousPage();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextPage();
            }
        }
    }

    /// <summary>
    /// Abre el inventario al presionar el input, en este caso "I"
    /// </summary>
    private void ToggleInventory()
    {
        if (OpenInventoryInput())
        {
            inventoryOpened = !inventoryOpened;
            inventoryPanel.SetActive(inventoryOpened); // Activa y desactiva el panel de el canvas

            if (inventoryOpened)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                UpdateInventory(); // En caso de que se este abriendo el inventario, lo actualiza, es decir, agrega los items nuevos que hayamos recogido
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private void UpdateInventory()
    {
        int itemsPerPage = 4; // Máximo de elementos por página
        int startIndex = actualPage * itemsPerPage; // Índice inicial de la página actual
        int endIndex = Mathf.Min(startIndex + itemsPerPage, inventario._Inventario.Count); // Índice final de la página actual

        // Desactiva todos los elementos previamente instanciados
        foreach (var item in itemsInstanciados)
        {
            if (item != null)
                item.SetActive(false);
        }

        // Posiciones específicas para los elementos
        Vector3[] posiciones = new Vector3[]
        {
            new Vector3(-1433, -348, 0),
            new Vector3(-1433, -715, 0),
            new Vector3(-651, -348, 0),
            new Vector3(-651, -706, 0)
        };

        // Instancia y posiciona los elementos de la página actual
        for (int i = startIndex; i < endIndex; i++)
        {
            if (uiItem == null)
            {
                Debug.LogError("El prefab uiItem no está asignado.");
                return;
            }

            if (itemsInstanciados[i] == null) // Si el elemento no ha sido instanciado aún
            {
                GameObject newUiItem = Instantiate(uiItem);
                newUiItem.transform.SetParent(instanceDestination.transform, false); // Emparenta respetando el layout
                newUiItem.transform.localScale = Vector3.one; // Asegura la escala

                // Asigna la posición específica
                int localIndex = i % itemsPerPage; // Índice relativo dentro de la página
                newUiItem.transform.localPosition = posiciones[localIndex];

                UIItem uiItemComponent = newUiItem.GetComponent<UIItem>();
                if (uiItemComponent == null)
                {
                    Debug.LogError("El prefab uiItem no tiene el componente UIItem.");
                    Destroy(newUiItem);
                    return;
                }

                uiItemComponent.SetItemInfo(inventario._Inventario[i]);
                itemsInstanciados[i] = newUiItem;
            }

            // Activa el elemento
            itemsInstanciados[i].SetActive(true);
        }
    }

    public void NextPage()
    {
        actualPage++;

        actualPage = Mathf.Clamp(actualPage, 0, 2); // Limita a 3 páginas máximo (0,1,2)
        UpdateInventory(); // Actualiza la UI
    }

    public void PreviousPage()
    {
        actualPage--;

        actualPage = Mathf.Clamp(actualPage, 0, 2); // Evita ir por debajo de la primera página
        UpdateInventory(); // Actualiza la UI
    }

    private bool OpenInventoryInput()
    {
        return Input.GetKeyDown(KeyCode.I);
    }
}

