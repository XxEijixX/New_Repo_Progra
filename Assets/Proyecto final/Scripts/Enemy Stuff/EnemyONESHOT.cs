using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyONESHOT : MonoBehaviour
{
    private Transform thePlayer;

    [Header("Explosión")]
    [SerializeField] private float radio = 3f;
    public LayerMask playerMask;
    public int damageToPlayer = 20;
    public ParticleSystem explosionEffect;

    [Header("Vida")]
    public int maxHealth = 100;
    private int currentHealth;
    public float pushBackForce = 5f;

    [Header("Sonido")]
    public AudioClip explosionSound;
    public AudioClip deathSound;

    private PlayerHealth playerHealth;
    private bool hasExploded = false;

    private void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = thePlayer.GetComponent<PlayerHealth>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (hasExploded) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, radio, playerMask);
        if (hits.Length > 0)
        {
            Explode();
        }
    }

    public void TakeDamage(int damage)
    {
        if (hasExploded) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        Debug.Log("El enemigo ha muerto.");

        // Reproducir sonido de muerte
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        Destroy(gameObject);
    }

    private void Explode()
    {
        hasExploded = true;

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageToPlayer);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            Rigidbody playerRb = thePlayer.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 direction = thePlayer.position - transform.position;
                direction.y = 0;
                playerRb.AddForce(direction.normalized * pushBackForce, ForceMode.Impulse);
            }
        }

        if (explosionEffect != null)
        {
            ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            explosion.Play();
            Destroy(explosion.gameObject, 2f);
        }

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
