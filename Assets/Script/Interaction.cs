using System;
using Unity.VisualScripting;
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
    private void Awake()
    {
        playerControls = new PlayerControls();
        pressEUI.SetActive(false);
        loadingScreenUI.SetActive(false);
        killAllToExtract.SetActive(false);
        currentLevelName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pressEUI.SetActive(false);
            killAllToExtract.SetActive(false);
        }
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
