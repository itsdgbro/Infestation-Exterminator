using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private bool isGamePaused = false;

    public bool GetIsGamePaused() => isGamePaused;

    [Header("Main Menu To Load")]
    [SerializeField] private string mainMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject gameOverUI;

    [Header("Weapon Ammo Count")]
    [Header("AI Ammo UI")]
    [SerializeField] private TextMeshProUGUI arCurrentAmmoUI;
    [SerializeField] private TextMeshProUGUI arRemainingAmmoUI;
    [Header("Pistol Ammo UI")]
    [SerializeField] private TextMeshProUGUI pistolCurrentAmmoUI;
    [SerializeField] private TextMeshProUGUI pistolRemainingAmmoUI;

    // reference to weapon data
    [Header("Weapon Data")]
    [SerializeField] private WeaponData arData;
    [SerializeField] private WeaponData pistolData;

    [Header("Reset on UnPause")]
    [SerializeField] private List<GameObject> pauseUIList;
    [SerializeField] private GameObject showPauseList;

    #region Player Controls
    private PlayerControls inputActions;
    #endregion


    private void Awake()
    {
        SetCursorState();
        pauseMenu.SetActive(false);
        gameOverUI.SetActive(false);
        inputActions = new PlayerControls();
    }


    private void Start()
    {
        ResumeGame();
    }

    // Update is called once per frame
    private void Update()
    {
        WeaponAmmoTextUI();
        if (isGamePaused)
        {
            inputActions.Disable();
        }
        else
        {
            inputActions.Enable();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseState();
        }
        // ZombieCountUI();
    }

    private void SetCursorState()
    {
        if (!isGamePaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void PauseGame()
    {
        // Debug.Log("Paused");
        pauseMenu.SetActive(true);
        showPauseList.SetActive(true);
        foreach (var obj in pauseUIList)
        {
            obj.SetActive(false);
        }
        isGamePaused = true;
        Time.timeScale = 0.0f;
        SetCursorState();
    }

    public void ResumeGame()
    {
        // Debug.Log("Resumee");
        pauseMenu.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1.0f;
        SetCursorState();
    }

    private void TogglePauseState()
    {
        if (isGamePaused)
        {
            inputActions.Enable();
            ResumeGame();
        }
        else
        {
            inputActions.Disable();
            PauseGame();
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    // update weapon ammo in UI
    private void WeaponAmmoTextUI()
    {

        // Update UI for AR
        arCurrentAmmoUI.text = arData.currentAmmo.ToString();
        arRemainingAmmoUI.text = arData.ammoLeft.ToString();

        // Update UI for Pistol
        pistolCurrentAmmoUI.text = pistolData.currentAmmo.ToString();
        pistolRemainingAmmoUI.text = pistolData.ammoLeft.ToString();


    }

    public void SaveGame()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void ShowDeadUI()
    {
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(index + " aaa");
        DataPersistenceManager.instance.SelectedLevelIndex = index;
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    #region Enable/Disable
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
    #endregion
}
