using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightLeft : MonoBehaviour
{

    public float moveSpeed = 5;
    public float damageXPosition = -0.5f;  // Set the x-coordinate where damage should occur


    void Start()
    {
        
    }

 
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();

        // Check if the ship is within the damage range
        if (transform.position.x < damageXPosition)
        {
            DamageShip();
        }
    }

    void Move()
    {
        Vector2 pos = transform.position;
        pos.x -= moveSpeed * Time.fixedDeltaTime;

        if (pos.x < -0.5)
        {
            Destroy(gameObject);
        }

        transform.position = pos;
    }

    void DamageShip()
    {
        // Assuming the ship has a script named "Ship" attached
        Ship ship = FindObjectOfType<Ship>();

        if (ship != null)
        {
            // Call the Hit method on the ship
            ship.Hit(gameObject);
        }

        // Optionally, you can destroy the object after causing damage
        Destroy(gameObject);
    }

}
