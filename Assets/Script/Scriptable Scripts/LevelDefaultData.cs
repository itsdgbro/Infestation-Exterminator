using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelDefaultData", menuName = "LevelData/LevelDefaultData")]
public class LevelData : ScriptableObject
{
    [Header("Scene Name")]
    public string sceneName;

    [Header("Player Data")]
    public int playerHealth;
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector3 playerDirection;
    public int healingPillsLeft;

}
