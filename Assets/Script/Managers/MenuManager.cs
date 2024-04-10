using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Levels To Load")]
    [SerializeField] private string[] levels;
    [SerializeField] private GameObject savedDataNotFoundUI;


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

        DataPersistenceManager.instance.NewGame();

        Debug.Log("CReated");
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

    //// Load game from saved data
    //public void LoadGame()
    //{
    //    // load any saved data from file
    //    GameData loadedGameData = DataPersistenceManager.instance.GetFileDataHandler().Load();

    //    // check if saved data exist
    //    if (loadedGameData == null || string.IsNullOrEmpty(loadedGameData.sceneName))
    //    {
    //        Debug.Log("No data was found.");

    //        savedDataNotFoundUI.SetActive(true);
    //        return;
    //    }

    //    // push the loaded data to all script that need it 
    //    foreach (IDataPersistence dataPersistence in DataPersistenceManager.instance.GetDataPersistenceObjects())
    //    {
    //        dataPersistence.LoadData(loadedGameData);
    //    }
    //    SceneManager.LoadSceneAsync(loadedGameData.sceneName);
    //}

    public  void LoadSavedGame()
    {
        if (DataPersistenceManager.instance.gameData == null)
        {
            savedDataNotFoundUI.SetActive(true);
            return;
        }
        SceneManager.LoadSceneAsync(DataPersistenceManager.instance.gameData.sceneName);
    }
}
