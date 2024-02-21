using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, ISTarget
{
    #region Target Health
    [SerializeField] private float health = 50f;
    #endregion

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
            Destroy(gameObject);
    }
}
