using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Zombie/ZombieData")]
public class ZombieData : ScriptableObject
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;


    public LayerMask targetMask;
    public LayerMask obstacleMask;
}
