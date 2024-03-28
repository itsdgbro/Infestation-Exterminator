using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Levels To Load")]
    [SerializeField] private string[] levels;

    private string loadSavedLevel;


    [SerializeField] private GameObject noSavedDataDialouge = null;

    public void LoadLevel(int index)
    {
        // check if index value exceeds the number of scenes 
        if (index >= SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogError("Invalid index: " + index);
            return;
        }

        // returns -1 if scene not found
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(levels[index]);

        if (buildIndex < 0)
        {
            Debug.LogError("Scene not found.");
            return;
        }

        SceneManager.LoadScene(levels[index]);
    }

    public void LoadGameDialouge()
    {
        if (PlayerPrefs.HasKey("SavedData"))
        {
            loadSavedLevel = PlayerPrefs.GetString("SavedData");
            SceneManager.LoadScene(loadSavedLevel);
        }
        else
        {
            noSavedDataDialouge.SetActive(true);
        }
    }

    public void OnLevelSelectButtonClick()
    {
        Debug.Log("Level");
    }

    public void StopTime()
    {
        Time.timeScale = 0.0f;
    }

    public void ResumeTime()
    {
        Time.timeScale = 1.0f;
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
