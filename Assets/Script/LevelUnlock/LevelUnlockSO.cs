using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelUnlockData", menuName = "LevelData/LevelUnlockData")]
public class LevelUnlockSO : ScriptableObject
{
    public bool isLevel2Unlocked;
    public bool isLevel3Unlocked;
}
