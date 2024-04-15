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
    PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        pressEUI.SetActive(false);
        loadingScreenUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && zombieCountManager.GetZombieAlive() <= 0)
        {
            pressEUI.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressEUI.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("DDDD" + DataPersistenceManager.instance.GetAliveZombieCount());
        if (other.gameObject.CompareTag("Player") && pressEUI.activeSelf)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                playerControls.Disable();
                loadingScreenUI.SetActive(true);
                SceneManager.LoadSceneAsync("Scene Menu");
            }
        }
    }
}
