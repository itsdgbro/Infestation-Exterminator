using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVelocityController : MonoBehaviour
{

    private bool isAttacking;

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

    private void Update()
    {
        Debug.Log("State " + isAttacking);
    }

}
