using UnityEngine;

public class EnemyVelocityController : MonoBehaviour
{

    Animator animator;
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        if (enemyAttack == null)
            Debug.LogWarning("Enemy Attack Scrpit not found");
    }

    public float AttackAnimationLen()
    {
        return animator.GetCurrentAnimatorStateInfo(1).length;
    }

    // public bool IsAttackAnimationPlaying()
    // {
    //     float normalizedTime = animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
    //     return animator.GetLayerName(1) == "Attack Layer" && normalizedTime < 1.0f;
    // }

    public bool IsAttackAnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(1).IsName("attack");
    }

    public void AttackTrigger()
    {
        enemyAttack.ApplyDamage();
    }

}
