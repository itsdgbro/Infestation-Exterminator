using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTipsUI : MonoBehaviour
{
    [SerializeField] private GameObject tipsUI;
    [SerializeField] private GameObject objectiveUI;


    private void Awake()
    {
        tipsUI.SetActive(true);
        objectiveUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            tipsUI.SetActive(false);
            objectiveUI.SetActive(true);
        }
    }
}
