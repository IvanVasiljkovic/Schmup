using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import the UnityEngine.UI namespace for Text

public class Ship : MonoBehaviour
{
    Vector2 initialPosition;

    Gun[] guns;

    int hits = 3;
    bool invincible = false;
    float invincibleTimer = 0;
    float invincibleTime = 2;

    bool shoot;
    public Text hpText; // Public Text field for HP display

    SpriteRenderer spriteRenderer;

    GameObject shield;
    int powerUpGunLevel = 0;



    public float speed = 5f;
    private Rigidbody2D rb;

    float damageXPosition = -0.5f;  // Set the x-coordinate where damage should occur

    private void Awake()
    {
        initialPosition = transform.position;
       
    }

    void Start()
    {
        shield = transform.Find("Shield").gameObject;
        DeactivateShield();

        guns = transform.GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            gun.isActive = true;
            if (gun.powerUpLevelRequirement != 0)
            {
                gun.gameObject.SetActive(false);
            }
        }

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateHPText(); // Call the method to update HP text
    }



    void Update()
    {
        MoveShip();

        // Check if any enemy has crossed the damage position
        if (transform.position.x < damageXPosition)
        {
            DamageShip();
        }

        shoot = Input.GetKeyDown(KeyCode.Space);
        if (shoot)
        {
            shoot = false;
            foreach (Gun gun in guns)
            {
                if (gun.gameObject.activeSelf)
                {
                    gun.Shoot();
                }
            }
        }

        if (invincible)
        {

            if (invincibleTimer >= invincibleTime)
            {
                invincibleTimer = 0;
                invincible = false;
                spriteRenderer.enabled = true;
            }
            else
            {
                invincibleTimer += Time.deltaTime;
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            UpdateHPText(); // Call the method to update HP text
        }

    }

    void UpdateHPText()
    {
        if (hpText != null)
        {
            hpText.text = "Lives: " + hits.ToString(); // Update the HP text
        }
    }

    void DamageShip()
    {
        Hit(gameObject);
    }




    void MoveShip()
    {
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement.y = -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement.x = 1;
        }

        // Normalize if moving diagonally
        movement.Normalize();

        // Calculate the target position based on the movement and speed
        Vector2 targetPosition = (Vector2)transform.position + movement * speed * Time.deltaTime;

        // Get the camera's boundaries
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        // Get the camera's position
        float cameraX = Camera.main.transform.position.x;
        float cameraY = Camera.main.transform.position.y;

        // Calculate the minimum and maximum positions within the camera view
        float minX = cameraX - cameraWidth + 0.5f;  // Adjust with the size of your ship
        float maxX = cameraX + cameraWidth - 0.5f;   // Adjust with the size of your ship
        float minY = cameraY - cameraHeight + 0.5f; // Adjust with the size of your ship
        float maxY = cameraY + cameraHeight - 0.5f;  // Adjust with the size of your ship

        // Clamp the target position to stay within the camera view
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

        // Set the velocity based on clamped target position
        rb.velocity = (targetPosition - (Vector2)transform.position) / Time.deltaTime;
    }


    void ActiveShield()
    {
        shield.SetActive(true);
    }

    void DeactivateShield()
    {
        shield.SetActive(false);
    }

    bool HasShield()
    {
        return shield.activeSelf;
    }

    void addGuns()
    {
        powerUpGunLevel++;
        foreach (Gun gun in guns)
        {
            if (gun.powerUpLevelRequirement <= powerUpGunLevel)
            {
                gun.gameObject.SetActive(true);
            }
            else
            {
                gun.gameObject.SetActive(false);
            }
        }
    }

    public void ResetShip()
    {
        hits--;

        if (hits <= 0)
        {
            // Load the "GameOver" scene when hits reach 0
            SceneManager.LoadScene("GameOver");

            // Notify the Level script to perform cleanup
            Level.instance.OnGameOver();
        }
        else
        {
            transform.position = initialPosition;
            DeactivateShield();
            powerUpGunLevel = -1;
            addGuns();

            // Reset the level if there are still hits left
            Level.instance.ResetLevel();
        }

        UpdateHPText(); // Call the method to update HP text
    }





    public void Hit(GameObject gameObjectHit)
    {
        if (HasShield())
        {
            DeactivateShield();
        }
        else
        {
            if (!invincible)
            {
                hits--;
                if (hits == 0)
                {
                    ResetShip();
                }
                else
                {
                    invincible = true;
                }
                Destroy(gameObjectHit);
            }
        }

        UpdateHPText(); // Call the method to update HP text
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            if (bullet.isEnemy)
            {

                Hit(bullet.gameObject);
            }

        }

        Destructable destructable = collision.GetComponent<Destructable>();
        if (destructable != null)
        {

            Hit(destructable.gameObject);
        }


        PowerUp powerUp = collision.GetComponent<PowerUp>();
        if (powerUp)
        {
            if (powerUp.activateShield)
            {
                ActiveShield();
            }
            if (powerUp.addGuns)
            {
                addGuns();
            }
            Destroy(powerUp.gameObject);
        }
    }

}