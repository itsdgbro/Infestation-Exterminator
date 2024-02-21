using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVelocityController : MonoBehaviour
{
    private bool isAttacking;

    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }

    public void PerformingAttack()
    {
        isAttacking = true;
    }

    public void EndingAttack()
    {
        isAttacking = false;
    }

    private void Update()
    {
        Debug.Log("State "+ isAttacking);
    }
}
