using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player/PlayerData")]
public class PlayerData : ScriptableObject
{

    [Header("Players Active Health")]
    [Range(0f, 100f)]
    public float playerHealth;

    [Header("Stamina")]
    public float stamina;
    public float staminaChangeRate;

    [Header("Is Dead")]
    public bool isDead;

}
