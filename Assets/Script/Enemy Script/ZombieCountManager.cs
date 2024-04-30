using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieCountManager : MonoBehaviour, IDataPersistence
{
    [Header("Zombie CountUI")]
    [SerializeField] private TextMeshProUGUI zombieCountUI;

    #region Zombie count 
    private List<GameObject> zombieList = new();
    private int totalZombies;   // total dead and undead
    private int zombieAlive;    // number of zombie alive
    public int GetZombieAlive() => zombieAlive;

    // remove specific zombie from the list
    public void SetZombieAlive(GameObject gameObject)
    {
        zombieList.Remove(gameObject);
        ZombieCountUI();
    }
    #endregion

    private void Start()
    {
        totalZombies = transform.childCount;
        AppendZombieList();
        ZombieCountUI();
    }

    // add zombie to a list
    private void AppendZombieList()
    {
        zombieList.Clear();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Target") && child.gameObject.activeSelf)
            {
                zombieList.Add(child.gameObject);
            }
        }
        if (zombieList.Count == 0)
        {
            zombieAlive = 0;
        }
    }

    // update current alive zombie and UI
    private void ZombieCountUI()
    {
        zombieAlive = zombieList.Count;
        zombieCountUI.text = zombieAlive.ToString() + "/" + totalZombies.ToString();
    }

    public void LoadData(GameData data)
    {
        this.zombieAlive = data.enemy.zombieCount;
    }

    public void SaveData(GameData data)
    {
        data.enemy.zombieCount = this.zombieAlive;
    }
}
