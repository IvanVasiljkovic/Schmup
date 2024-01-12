using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // Hide the cursor
        Cursor.visible = false;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // You can add additional logic here if needed
    }
}