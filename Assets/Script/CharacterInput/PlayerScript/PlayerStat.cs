using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    
    [SerializeField] private PlayerData playerData;
    private float p_Health = 100;

    public void ReceiveDamage(float damage)
    {
        p_Health -= damage;
        if (p_Health <= 0)
        {
            // player die
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        Debug.Log("Player Health " +  p_Health);
    }
}
