using UnityEngine;

[CreateAssetMenu(fileName = "Pills", menuName = "Healing/Pills")]
public class HealData : ScriptableObject
{
    [Header("Item Name")]
    public string healItemName;
    public float healAmount;
    public int availablePills;
}
