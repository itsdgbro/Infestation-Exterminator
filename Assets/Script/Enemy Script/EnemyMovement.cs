using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float movementSpeed = 5f; // Adjust the speed as needed
    public float detectionRadius = 10f; // Radius within which the player triggers enemy movement
    private Transform player;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure to tag your player GameObject with 'Player'");
        }
    }

    void Update()
    {
        // Check the distance between the enemy and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within the detection radius or there is a direct line of sight, move towards the player
        if (distanceToPlayer <= detectionRadius || HasDirectLineOfSight())
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move towards the player
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

        // Face the player
        transform.LookAt(player.position);
    }

    bool HasDirectLineOfSight()
    {
        if (player == null)
        {
            return false;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.position - transform.position, out hit, detectionRadius))
        {
            // Check if the ray hit the player
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    // Draw Gizmos in the Scene view for better visualization
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw the raycast line only if the player is not null
        if (player != null)
        {
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
