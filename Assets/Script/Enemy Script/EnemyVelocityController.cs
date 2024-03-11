using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVelocityController : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AttackPoint()
    {

    }

    public bool IsAttackAnimationPlaying()
    {
        return animator.GetLayerName(1) == "Attack Layer" && animator.GetCurrentAnimatorStateInfo(1).IsName("attack");
    }

    public void DestroyZombie()
    {
        
    }

    private void Update()
    {
        /*Debug.Log("State " + isAttacking);*/
    }

}
