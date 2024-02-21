using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    /*[SerializeField] private ZombieData m_Data;
    [SerializeField] private PlayerData m_PlayerData;*/


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Zombie attack");
        }
    }
}
