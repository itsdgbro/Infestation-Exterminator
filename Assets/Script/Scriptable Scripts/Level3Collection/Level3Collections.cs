using UnityEngine;

[CreateAssetMenu(fileName = "Level3", menuName = "LevelObjective/Level3")]
public class Level3Collections : ScriptableObject
{
    public bool[] isCollected = new bool[3];
}
