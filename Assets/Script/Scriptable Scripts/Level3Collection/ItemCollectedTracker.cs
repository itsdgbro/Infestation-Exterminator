using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectedTracker : MonoBehaviour
{
    private GameObject[] collectableObjects = new GameObject[3];
    [SerializeField] private Level3Collections level3Collections;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            collectableObjects[i] = transform.GetChild(i).gameObject;
            collectableObjects[i].SetActive(!level3Collections.isCollected[i]);
        }
    }


}
