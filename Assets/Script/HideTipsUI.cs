using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTipsUI : MonoBehaviour
{
    [SerializeField] private GameObject tipsUI;

    private void Awake()
    {
        tipsUI.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            tipsUI.SetActive(false);
        }
    }
}
