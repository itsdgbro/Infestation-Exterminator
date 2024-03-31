using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [System.Serializable]
    public class PlayerData
    {

        public float health;

        public Vector3 positioln; // player's position
        public Quaternion rotation; // Store player's rotation
        public Vector3 direction; // Store player's forward direction

        public int healingPillsLeft;
    }

    [System.Serializable]
    public class EnemyData
    {
        public int zombieCount;
        // list of zombie dead or alive
        public SerializableDictionary<string, bool> isZombieDead;

    }

    public PlayerData player;
    public EnemyData enemy;
    public GameData()
    {
        this.player = new PlayerData
        {
            health = 100f,
            positioln = Vector3.zero,
            rotation = Quaternion.identity,
            direction = Vector3.forward,
            healingPillsLeft = 5
        };

        this.enemy = new EnemyData
        {
            zombieCount = 0,
            isZombieDead = new SerializableDictionary<string, bool>()
        };
    }


}
