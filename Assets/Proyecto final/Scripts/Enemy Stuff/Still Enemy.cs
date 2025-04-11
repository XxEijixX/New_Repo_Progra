using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StillEnemy : MonoBehaviour
{
    [Header("Configuración de Explosión")]
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int damageToPlayer = 20;
    [SerializeField] private float pushBackForce = 5f;
    [SerializeField] private ParticleSystem explosionEffect;
    [Header("Sonido")]
    public AudioClip explosionSound;  // Sonido de explosión

    [Header("Configuración de Vida")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool hasExploded = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (hasExploded) return;

        // Verificación más precisa del jugador
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth player = hitCollider.GetComponent<PlayerHealth>();
                if (player != null)
                {
                    Explode();
                    break;
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (hasExploded) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Aplicar daño a todos los jugadores en el radio
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    Debug.Log("Aplicando daño al jugador"); // Mensaje de depuración
                    playerHealth.TakeDamage(damageToPlayer);

                    // Empujar al jugador
                    Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                        direction.y = 0;
                        rb.AddForce(direction * pushBackForce, ForceMode.Impulse);
                    }
                }
            }
        }

        // Efecto visual
        if (explosionEffect != null)
        {
            ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            explosion.Play();
            Destroy(explosion.gameObject, explosion.main.duration);
        }

        // Reproducir sonido de explosión
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        Destroy(gameObject, 0.1f); // Pequeño delay para asegurar que se aplica el daño
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
