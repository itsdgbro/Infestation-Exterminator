using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Main Menu To Load")]
    [SerializeField] private string mainMenu;
    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inGameUI;

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

    private void Awake()
    {
        SetCursorState();
        pauseMenu.SetActive(false);
    }

    private void Start()
    {
        ResumeGame();
    }

    // Update is called once per frame
    private void Update()
    {
        WeaponAmmoTextUI();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseState();
        }
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
        Time.timeScale = 0.0f;
        isGamePaused = true;
        SetCursorState();
    }

    public void ResumeGame()
    {
        // Debug.Log("Resumee");
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isGamePaused = false;
        SetCursorState();
    }

    private void TogglePauseState()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void GoTOMainMenu()
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
