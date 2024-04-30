using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Interaction : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pressEUI;
    [SerializeField] private ZombieCountManager zombieCountManager;
    [SerializeField] private GameObject loadingScreenUI;
    [SerializeField] private GameObject killAllToExtract;

    [Header("Level Unlock Dat")]
    [SerializeField] private LevelUnlockSO levelUnlockSO;
    private String currentLevelName;
    private readonly String[] levelList = { "Level 1", "Level 2", "Level 3" };
    PlayerControls playerControls;

    [Header("Level 3 collectables")]
    [SerializeField] private Level3Collections level3Collections;
    private void Awake()
    {
        playerControls = new PlayerControls();
        pressEUI.SetActive(false);
        loadingScreenUI.SetActive(false);
        killAllToExtract.SetActive(false);
        currentLevelName = SceneManager.GetActiveScene().name;
        if (level3Collections == null && currentLevelName == "Level 3")
        {
            Debug.LogError("Level 3 collectables not found.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentLevelName != "Level 3")
        {
            if (other.gameObject.CompareTag("Player") && zombieCountManager.GetZombieAlive() <= 0)
            {
                pressEUI.SetActive(true);
            }
            else if (other.gameObject.CompareTag("Player") && zombieCountManager.GetZombieAlive() > 0)
            {
                killAllToExtract.SetActive(true);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (zombieCountManager.GetZombieAlive() <= 0 && IsAllCollected())
                {
                    pressEUI.SetActive(true);
                }
                else if (zombieCountManager.GetZombieAlive() > 0 || !IsAllCollected())
                {
                    killAllToExtract.SetActive(true);
                }
            }
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

    private bool IsAllCollected()
    {

        foreach (var item in level3Collections.isCollected)
        {
            if (item == false)
            {
                return false;
            }
        }

        return true;
    }

    private void Update()
    {
        if (pressEUI.activeSelf && playerControls.Interactive.Interact.triggered)
        {
            if (currentLevelName == levelList[0])
            {
                levelUnlockSO.isLevel2Unlocked = true;
            }
            else if (currentLevelName == levelList[1])
            {
                levelUnlockSO.isLevel3Unlocked = true;
            }
            loadingScreenUI.SetActive(true);
            SceneManager.LoadSceneAsync("Scene Menu");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            playerControls.Disable();
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
