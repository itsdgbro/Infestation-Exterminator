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

            // Press E to evacuate 
            if (playerControls.Interactive.Interact.triggered)
            {
                loadingScreenUI.SetActive(true);
                SceneManager.LoadSceneAsync("Scene Menu");
                levelUnlockSO.isLevel2Unlocked = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                playerControls.Disable();
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
