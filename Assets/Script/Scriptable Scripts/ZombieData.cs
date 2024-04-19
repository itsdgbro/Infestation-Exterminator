using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Zombie/ZombieData")]
public class ZombieData : ScriptableObject
{
    [Header("Primary Attributes")]
    public float attackDamage;
    public float maxRange;
    public float attackCooldown;

    [Header("FOV Attributes")]
    public float viewRadius;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask groundMask;
}
