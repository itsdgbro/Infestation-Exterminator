using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string sceneName;

    [System.Serializable]
    public class PlayerData
    {

        public float health;

        public Vector3 position; // player's position
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
        this.sceneName = "Level 1";
        this.player = new PlayerData
        {
            health = 100f,
            position = new(467.55f, 25.52f, 162.92f),
            rotation = new Quaternion(0, 0.451710999f, 0, 0.89216429f),
            direction = new(0.9085726141929627f, 0f, 0.41772717237472536f),
            healingPillsLeft = 5
        };

        this.enemy = new EnemyData
        {
            zombieCount = 0,
            isZombieDead = new SerializableDictionary<string, bool>()
        };
    }


}
