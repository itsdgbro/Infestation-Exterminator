using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Levels To Load")]
    [SerializeField] private string[] levels;
    [SerializeField] private GameObject savedDataNotFoundUI;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private HideTipsSO hideTips;

    string path;
    string fileName;

    private void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath;
        fileName = DataPersistenceManager.instance.GetFileName();
    }

    private void OnEnable()
    {
        if (!File.Exists(DoFileExist()))
        {
            loadGameButton.interactable = false;
        }
    }

    private void Start()
    {
        savedDataNotFoundUI.SetActive(false);
    }

    // load the selected level from UI 
    public void LoadNewLevel(int index)
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
        
        // unhide Tips hub
        hideTips.hideTips = false;
        SceneManager.LoadSceneAsync(levels[index]);
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

    public void LoadSavedGame()
    {
        // hide Tips hub
        hideTips.hideTips = true;
        if (!DataPersistenceManager.instance.HasGameData())
        {
            savedDataNotFoundUI.SetActive(true);
            return;
        }
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.SavedLevelName());
    }

    public string DoFileExist()
    {
        return Path.Combine(path, fileName);
    }

    // trigger change to either load or set to default
    public void TriggerLoadGame(bool value)
    {
        DataPersistenceManager.instance.SetIsLoad(value);
        hideTips.hideTips = value;
    }

    public void SetLevelSelectIndex(int index)
    {
        DataPersistenceManager.instance.SelectedLevelIndex = index; 
    }

}
