using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pills", menuName = "Healing/Pills")]
public class HealData : ScriptableObject
{
    [Header("Item Name")]
    public string healItemName;
    public float healAmount;

}
