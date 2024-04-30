using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCollectedTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI collectTextUI;
    private GameObject[] collectableObjects = new GameObject[3];
    [SerializeField] private Level3Collections level3Collections;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            collectableObjects[i] = transform.GetChild(i).gameObject;
            collectableObjects[i].SetActive(!level3Collections.isCollected[i]);
            ItemCollectedCounter();
        }
    }

    public void ItemCollectedCounter()
    {
        int collected = 0;
        foreach (var item in level3Collections.isCollected)
        {
            if (item == true)
            {
                collected++;
            }
        }
        collectTextUI.text = collected.ToString() + "/3";
    }


}
