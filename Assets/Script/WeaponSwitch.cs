using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] weaponDraw;
    private int selectedWeapon;
    private float timeSinceLastSwitch;

    [Header("Weapon UI")]
    [SerializeField] private GameObject[] weaponUI;

    private void Start()
    {   
        audioSource = GetComponent<AudioSource>();
        SetWeapons();
        Select(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }

    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            weapons[i] = transform.GetChild(i);

        keys ??= new KeyCode[weapons.Length];
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        for (int i = 0; i < keys.Length; i++)
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
                selectedWeapon = i;

        if (previousSelectedWeapon != selectedWeapon) Select(selectedWeapon);

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++) { 
            weapons[i].gameObject.SetActive(i == weaponIndex);
            weaponUI[i].SetActive(i == weaponIndex);
        }
        timeSinceLastSwitch = 0f;
        // Play the draw sound when a new weapon is selected
        if (audioSource != null && weaponDraw != null)
        {
            audioSource.PlayOneShot(weaponDraw[weaponIndex]);
        }
        OnWeaponSelected();
    }

    private void OnWeaponSelected() { }
}