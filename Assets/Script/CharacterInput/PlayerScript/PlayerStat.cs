using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private float currentHealth = 0;

    private void Awake()
    {
        currentHealth = playerData.health;
    }
    
    public void ReceiveDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void PlayerDie()
    {
        if (currentHealth <= 0) {
            // player die
            Destroy(gameObject);
        }
    }
}
