using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDestructible : MonoBehaviour
{
    public float damageXPosition = -0.5f;  // Set the x-coordinate where damage should occur
    public GameObject powerUpPrefab;
    [Header("Destruction Settings")]
    public int scoreValue = 100;
    public int maxHealth = 10; // Set the maximum health for the destructible object

    private int currentHealth;
    private bool canBeDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        Level.instance.AddDestructable();
        currentHealth = maxHealth; // Initialize current health with max health
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 17.5f && !canBeDestroyed)
        {
            canBeDestroyed = true;
            // Additional initialization if needed for this specific enemy
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeDestroyed)
        {
            return;
        }

        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null && !bullet.isEnemy)
        {
            TakeDamage(1); // Assuming each bullet deals 1 damage
            Level.instance.AddScore(scoreValue);
            Destroy(bullet.gameObject);
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        Level.instance.RemoveDestructable();
        // Additional cleanup if needed for this specific enemy
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void DamagePlayerShip()
    {
        Ship playerShip = FindObjectOfType<Ship>();
        if (playerShip != null)
        {
            playerShip.Hit(gameObject);
        }
    }

    void DropPowerUp(Vector3 spawnPosition)
    {
        if (powerUpPrefab != null)
        {
            Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
