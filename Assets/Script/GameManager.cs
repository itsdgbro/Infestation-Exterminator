using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Main Menu To Load")]
    [SerializeField] private string mainMenu;
    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;


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
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            Debug.Log(isGamePaused);
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
        Debug.Log("Paused");
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isGamePaused = true;
        SetCursorState();
    }

    public void ResumeGame()
    {
        Debug.Log("Resumee");
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


    public void QuitGame()
    {
        Application.Quit();
    }
}
