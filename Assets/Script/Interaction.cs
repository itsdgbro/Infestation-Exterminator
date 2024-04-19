using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{
    [SerializeField] private GameObject pressEUI;
    [SerializeField] private ZombieCountManager zombieCountManager;
    [SerializeField] private GameObject loadingScreenUI;
    [SerializeField] private GameObject killAllToExtract;
    PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        pressEUI.SetActive(false);
        loadingScreenUI.SetActive(false);
        killAllToExtract.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && zombieCountManager.GetZombieAlive() <= 0)
        {
            pressEUI.SetActive(true);
        }
        else
        {
            killAllToExtract.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressEUI.SetActive(false);
            killAllToExtract.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("DDDD" + DataPersistenceManager.instance.GetAliveZombieCount());
        if (other.gameObject.CompareTag("Player") && pressEUI.activeSelf)
        {
            if (playerControls.Interactive.Interact.triggered)
            {
                playerControls.Disable();
                loadingScreenUI.SetActive(true);
                SceneManager.LoadSceneAsync("Scene Menu");
            }
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
