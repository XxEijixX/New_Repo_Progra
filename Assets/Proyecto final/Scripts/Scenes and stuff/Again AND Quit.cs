using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AgainANDQuit : MonoBehaviour
{
    // M�todo para recargar la escena 
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame"); // Carga la escena 
    }

    // M�todo para salir del juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego..."); // Para verificar en el editor
        Application.Quit(); // Cierra la aplicaci�n
    }
}
