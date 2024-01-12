using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float targetX; // Set this value in the Inspector
    public float upDownRange = 2f; // Set the range for up and down movement
    public float moveSpeed = 5f; // Adjust the overall movement speed
    public float minUpDownTime = 0.5f; // Minimum time between movements
    public float maxUpDownTime = 2f; // Maximum time between movements

    private bool movingUpDown = false;
    private float initialY;
    private float timeBetweenMovements;
    private Vector3 targetPosition;

    void Start()
    {
        initialY = transform.position.y;
        SetRandomTimeBetweenMovements();
    }

    void Update()
    {
        if (!movingUpDown)
        {
            MoveBoss();
            if (Mathf.Approximately(transform.position.x, targetX))
            {
                movingUpDown = true;
                targetPosition = new Vector3(transform.position.x, initialY + Random.Range(-upDownRange, upDownRange), transform.position.z);
            }
        }

        if (movingUpDown)
        {
            LerpToYPosition(targetPosition);
        }
    }

    void MoveBoss()
    {
        float step = moveSpeed * Time.deltaTime;
        Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    void LerpToYPosition(Vector3 targetPosition)
    {
        float lerpSpeed = 1f / timeBetweenMovements;
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            movingUpDown = false;
            SetRandomTimeBetweenMovements();
        }
    }

    void SetRandomTimeBetweenMovements()
    {
        timeBetweenMovements = Random.Range(minUpDownTime, maxUpDownTime);
    }
}
