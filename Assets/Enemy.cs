using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject powerUpPrefab;  // Drag your power-up prefab here in the Unity Editor
    public float damageXPosition = -0.5f;  // Set the x-coordinate where damage should occur
    public Ship playerShip;  // Reference to the player ship

    void Update()
    {
        // Move the enemy or perform other actions

        // Check if the enemy has crossed the damage position
        if (transform.position.x < damageXPosition)
        {
            // Call the Hit method on the player ship
            if (playerShip != null)
            {
                playerShip.Hit(gameObject);

                // Drop a power-up when the enemy is hit
                DropPowerUp();
            }
        }
    }

    void DropPowerUp()
    {
        if (powerUpPrefab != null)
        {
            Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        }
    }

    // Other methods and functionalities for the enemy script
}
