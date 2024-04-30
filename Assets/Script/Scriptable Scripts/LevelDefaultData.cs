using UnityEngine;


[CreateAssetMenu(fileName = "LevelDefaultData", menuName = "LevelData/LevelDefaultData")]
public class LevelData : ScriptableObject
{
    [Header("Scene Name")]
    public string sceneName;

    public float health;

    public Vector3 position; // player's position
    public Quaternion rotation; // Store player's rotation
    public Vector3 direction; // Store player's forward direction

    public int healingPillsLeft;

    [Header("AR Data")]
    public int aLeftAmmo;
    public int aCurrentAmmo;

    [Header("Pistol Data")]
    public int pLeftAmmo;
    public int pCurrentAmmo;
}
