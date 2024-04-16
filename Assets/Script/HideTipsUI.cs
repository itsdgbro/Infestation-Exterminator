using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTipsUI : MonoBehaviour
{
    [SerializeField] private GameObject initialTip;
    [SerializeField] private GameObject tipsUI;
    [SerializeField] private GameObject objectiveUI;
    [SerializeField] private HideTipsSO hideTipsSO;


    // show initial when collided
    // hide tips when true

    private void Awake()
    {
        initialTip.SetActive(false);
        tipsUI.SetActive(!hideTipsSO.hideTips);
        objectiveUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            initialTip.SetActive(true);
            Invoke(nameof(HideUiTips), 3.0f);
            objectiveUI.SetActive(true);
        }
    }

    public void HideUiTips()
    {

        initialTip.SetActive(false);
        tipsUI.SetActive(false);
        hideTipsSO.hideTips = false;
    }
}
