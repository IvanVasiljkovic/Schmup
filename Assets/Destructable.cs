using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    public float damageXPosition = -0.5f;  // Set the x-coordinate where damage should occur
    public GameObject powerUpPrefab;
    [Header("Destruction Settings")]
    public int scoreValue = 100;
    public int maxHits = 3; // Set the maximum hits for the destructible object

    private int currentHits = 0;
    private bool canBeDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        Level.instance.AddDestructable();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 17.5f && !canBeDestroyed)
        {
            canBeDestroyed = true;
            Gun[] guns = transform.GetComponentsInChildren<Gun>();
            foreach (Gun gun in guns)
            {
                gun.isActive = true;
            }
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
            TakeDamage();
            Level.instance.AddScore(scoreValue);
            Destroy(bullet.gameObject);

            if (currentHits >= maxHits)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        Level.instance.RemoveDestructable();
    }

    void TakeDamage()
    {
        currentHits++;
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
