using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    private PlayerStat stat;

    // references
    private EnemyVelocityController velocityController;
    private FieldOfView fov;

    #region Player Reference
    private GameObject player;
    private CapsuleCollider capsuleCollider;
    #endregion

    #region Zombie Reference
    #endregion
    public bool canAttack { get; set; }

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

        if (fov == null)
        {
            Debug.LogWarning("FOV not found");
        }
        capsuleCollider = GetComponentInParent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            Debug.LogWarning("Capsule not found");
            Debug.Log(capsuleCollider.gameObject.name);
        }
    }

    // perform attack
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && velocityController.IsAttackAnimationPlaying() && !canAttack)
        {
            ApplyDamange();
        }
    }

    // damage to player
    void ApplyDamange()
    {
        if (Vector3.Distance(player.transform.position, capsuleCollider.transform.position) < 0.89f)
        {
            stat.ReceiveDamage(zombieData.attackDamage);
        }
    }
}
