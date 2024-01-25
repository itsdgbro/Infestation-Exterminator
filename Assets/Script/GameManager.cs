using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;


    private void Awake()
    {
        SetCursorState();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
        pauseMenu.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0.0f;
        SetCursorState();
        SetCursorState();
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1.0f;
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
}
