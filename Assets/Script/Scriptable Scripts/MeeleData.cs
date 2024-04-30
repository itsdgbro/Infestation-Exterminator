using UnityEngine;
[CreateAssetMenu(fileName = "Meele", menuName = "Weapon/Meele")]
public class MeleeData : ScriptableObject
{

    [Header("Meele Info")]
    public new string name;

    [Header("Attributes")]
    public float damage;
    public float maxDistance;
    [Header("Cooldown in Seconds")]
    public float fireRate;
}
