using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    [SerializeField] private ZombieData zombieData;
    PlayerStat stat;

    private void Awake()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            stat = playerObject.GetComponent<PlayerStat>();
            if (stat == null)
            {
                Debug.LogError("PlayerStat component not found on the Player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stat.ReceiveDamage(zombieData.attackDamage);
        }
    }
}
