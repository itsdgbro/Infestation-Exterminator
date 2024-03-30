using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int zombieCount;

    public Vector3 playerPosition; // player's position
    public Quaternion playerRotation; // Store player's rotation
    public Vector3 playerForward; // Store player's forward direction

    public SerializableDictionary<string, bool> isZombieDead;

    public GameData()
    {
        this.zombieCount = 0;
        playerPosition = Vector3.zero;
        playerRotation = Quaternion.identity;
        playerForward = Vector3.forward;
        isZombieDead = new SerializableDictionary<string, bool>();
    }


}
