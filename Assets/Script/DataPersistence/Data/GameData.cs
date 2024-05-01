using UnityEngine;
using static GameData.WeaponData;

[System.Serializable]
public class GameData
{
    public string sceneName;

    // Player Data
    [System.Serializable]
    public class PlayerData
    {

        public float health;

        public Vector3 position; // player's position
        public Quaternion rotation; // Store player's rotation
        public Vector3 direction; // Store player's forward direction

        public int healingPillsLeft;
    }


    // Weapon Data
    [System.Serializable]
    public class WeaponData
    {
        // AR Data
        [System.Serializable]
        public class ARData
        {
            public int totalAmmo;
            public int currentAmmo;
        }

        // Pistol Data
        [System.Serializable]
        public class PistolData
        {
            public int totalAmmo;
            public int currentAmmo;
        }

        public ARData ar;
        public PistolData pistol;
    }

    // Zombie Data
    [System.Serializable]
    public class EnemyData
    {
        public int zombieCount;
        // list of zombie dead or alive
        public SerializableDictionary<string, bool> isZombieDead;

    }

    // level 3 item collected
    [System.Serializable]
    public class ItemCollected
    {
        public SerializableDictionary<string, bool> isItemCollected;
    }

    public PlayerData player;
    public WeaponData weapon;
    public EnemyData enemy;
    public ItemCollected itemCollected;
    public GameData(LevelData levelData)
    {
        this.sceneName = levelData.sceneName;

        this.player = new PlayerData
        {
            health = levelData.health,
            position = levelData.position,
            rotation = levelData.rotation,
            direction = levelData.direction,
            healingPillsLeft = levelData.healingPillsLeft
        };

        this.weapon = new WeaponData
        {

            ar = new ARData
            {
                totalAmmo = levelData.aLeftAmmo,
                currentAmmo = levelData.aCurrentAmmo,
            },
            pistol = new PistolData
            {
                totalAmmo = levelData.pLeftAmmo,
                currentAmmo = levelData.pCurrentAmmo,
            }
        };

        this.enemy = new EnemyData
        {
            zombieCount = 0,
            isZombieDead = new SerializableDictionary<string, bool>()
        };

        this.itemCollected = new ItemCollected
        {
            isItemCollected = new SerializableDictionary<string, bool>()
        };

    }


}
