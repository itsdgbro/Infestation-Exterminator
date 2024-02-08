using System.Collections;
using System.Collections.Generic;
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
    public float fireRate;
    public float reloadTime;
    public bool isAutomatic;

    [HideInInspector] public bool isReloading;
}
