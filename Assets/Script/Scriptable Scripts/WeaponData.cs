using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Ammo Data")]
    public int magazineSize;
    public int currentAmmo;
    public int ammoLeft;
    public float fireRate;
    public float reloadTime;
    public bool isAutomatic;

    [HideInInspector] public bool isReloading;
}
