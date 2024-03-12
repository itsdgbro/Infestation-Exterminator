using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVelocityController : MonoBehaviour
{

    Animator animator;
    private EnemyAttack enemyAttack;
    private SphereCollider sphereCollider = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        if (enemyAttack == null) 
            Debug.LogWarning("Enemy Attack Scrpit not found");

        sphereCollider = GetComponentInChildren<SphereCollider>();
        sphereCollider.enabled = false;

    }

    public void AttackPoint()
    {
        
    }

    public float AttackAnimationLen()
    {
        return animator.GetCurrentAnimatorStateInfo(1).length;
    }

    public bool IsAttackAnimationPlaying()
    {
        float normalizedTime = animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
        return animator.GetLayerName(1) == "Attack Layer" && normalizedTime < 1.0f;
    }


    public void DestroyZombie()
    {

    }

    #region Enable Zombie hande collider 
    public void EnableCollider()
    {
        sphereCollider.enabled = true;
    }

    public void DisableCollider()
    {
        sphereCollider.enabled = false;
    }
    #endregion
    private void Update()
    {
        
    }

}
