using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private ZombieData zombieData;
    [SerializeField] private PlayerStat stat;

    // references
    private EnemyVelocityController velocityController;
    private FieldOfView fov;

    #region Player Reference
    private GameObject player;
    private CapsuleCollider capsuleCollider;
    #endregion

    #region Zombie Reference
    #endregion

    private void Awake()
    {
        // check references is null
        if (stat == null)
        {
            Debug.LogError("Not Found");
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

    public bool canAttack = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && velocityController.IsAttackAnimationPlaying())
        {
            canAttack = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canAttack = true;
        }
    }

    // damage to player
    public void ApplyDamage()
    {
        if (Vector3.Distance(player.transform.position, capsuleCollider.transform.position) < 0.89f && canAttack)
        {
            stat.ReceiveDamage(zombieData.attackDamage);
            //canAttack = false;
        }
    }


}
