using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVelocityController : MonoBehaviour
{

    private bool isAttacking;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool GetIsAttacking() => isAttacking;
    public void SetIsAttacking(bool value) => isAttacking = value;

    public void PerformingAttack()
    {
        isAttacking = true;
    }

    public void EndingAttack()
    {
        isAttacking = false;
    }

    public void AttackPoint()
    {

    }

    public bool IsAttackAnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("attack");
    }

    private void Update()
    {
        /*Debug.Log("State " + isAttacking);*/
    }

}
