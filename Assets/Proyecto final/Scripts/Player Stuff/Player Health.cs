using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 150;
    private int currentHealth;

    public TextMeshProUGUI vidaTexto;
    private bool isDead = false;

    public Transform spawnPoint; // Asigna esto en el Inspector
    private PlayerMove movementScript;
    private CharacterController charController;
    private NavMeshAgent navAgent;

    private void Start()
    {
        currentHealth = maxHealth;
        ActualizarUI();

        movementScript = GetComponent<PlayerMove>();
        charController = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();

        if (spawnPoint == null)
        {
            Debug.LogWarning("No hay spawnPoint asignado. Usando posición inicial del jugador.");
            spawnPoint = new GameObject("DefaultSpawnPoint").transform;
            spawnPoint.position = transform.position;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        ActualizarUI();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void ActualizarUI()
    {
        if (vidaTexto != null)
            vidaTexto.text = "Vida: " + currentHealth;
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Jugador muerto. Reiniciando escena...");

        // Desactivar movimiento y controles
        if (movementScript != null) movementScript.enabled = false;
        if (navAgent != null) navAgent.enabled = false;
        if (charController != null) charController.enabled = false;

        // Reiniciar la escena después de un delay
        Invoke("ReiniciarEscena", 2f);
    }

    void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
