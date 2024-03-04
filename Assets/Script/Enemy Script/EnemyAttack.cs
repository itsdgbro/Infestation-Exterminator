using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    private PlayerStat stat;

    // references
    private EnemyVelocityController velocityController;
    private FieldOfView fov;
    private GameObject player;
    private CapsuleCollider capsuleCollider;

    private bool canDealDamage = true;
    private float damageCooldown = 2.6167f;  // Adjust the cooldown duration as needed
    private float lastDamageTime;
    private bool hasCollided = false;
    

    private void Awake()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            stat = playerObject.GetComponent<PlayerStat>();
            if (stat == null)
            {
                Debug.LogError("PlayerStat component not found on the Player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
        velocityController = GetComponentInParent<EnemyVelocityController>();
        fov = GetComponentInParent<FieldOfView>();
        player = fov.gameObject;

        if (velocityController == null)
        {
            Debug.LogWarning("Velocity script not found");
        }

        if(fov == null)
        {
            Debug.LogWarning("FOV not found");
        }
        capsuleCollider = GetComponentInParent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            Debug.LogWarning("Capsule not found");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && velocityController.IsAttackAnimationPlaying() && canDealDamage && !hasCollided)
        {
            hasCollided = true;
            StartCoroutine(ApplyDamageWithDelay());
        }
    }

    IEnumerator ApplyDamageWithDelay()
    {
        canDealDamage = false;

        yield return new WaitForSeconds(damageCooldown / 3.5f);

        if (Vector3.Distance(player.transform.position, capsuleCollider.transform.position) < 0.89f)
        {
            Debug.Log("DIstance");
            stat.ReceiveDamage(zombieData.attackDamage);
        }

        lastDamageTime = Time.time;
        canDealDamage = true;
        hasCollided = false;
    }

    private void Update()
    {
        if (!canDealDamage && Time.time - lastDamageTime > damageCooldown)
        {
            canDealDamage = true;
        }
    }
}
