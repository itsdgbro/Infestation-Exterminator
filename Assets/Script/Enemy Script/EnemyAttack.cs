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

    private readonly float damageCooldown = 1.6667f;  
    private float lastDamageAttack;
    

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
        if (other.gameObject.CompareTag("Player") && velocityController.IsAttackAnimationPlaying() && lastDamageAttack > damageCooldown)
        {
            Debug.Log("HELLOO");
            StartCoroutine(ApplyDamageWithDelay());
        }
    }

    IEnumerator ApplyDamageWithDelay()
    {
        yield return new WaitForSeconds(damageCooldown / 2f);

        if (Vector3.Distance(player.transform.position, capsuleCollider.transform.position) < 0.89f)
        {
            Debug.Log("DIstance");
            stat.ReceiveDamage(zombieData.attackDamage);
        }

        // reset attack cooldown
        lastDamageAttack = 0f;
    }

    private void Update()
    {
        lastDamageAttack += Time.deltaTime;
    }
}
