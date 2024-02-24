using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Levels To Load")]
    [SerializeField] private string newLevel;
    private string loadLevel;


    [SerializeField] private GameObject noSavedDataDialouge = null;
    public void NewGameDialouge()
    {
        SceneManager.LoadScene(newLevel);
    }

    public void LoadGameDialouge()
    {
        if (PlayerPrefs.HasKey("SavedData"))
        {
            loadLevel = PlayerPrefs.GetString("SavedData");
            SceneManager.LoadScene(loadLevel);
        }
        else
        {
            noSavedDataDialouge.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
