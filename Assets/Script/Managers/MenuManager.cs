using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Levels To Load")]
    [SerializeField] private string[] levels;
    [SerializeField] private GameObject savedDataNotFoundUI;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private HideTipsSO hideTipsSO;
    [SerializeField] private TextMeshProUGUI loadingLevelText;
    [SerializeField] private Slider sliderUI;

    string path;
    string fileName;

    private void Awake()
    {
        // DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath;
        if (DataPersistenceManager.instance == null)
        {
            Debug.LogError("NUL");
        }
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

    // load (New Game) the selected level from UI 
    public void LoadNewLevel(int index)
    {
        // check if index value exceeds the number of scenes 
        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("Invalid index: " + index);
            return;
        }

/*        // returns -1 if scene not found
        int buildIndex = SceneUtility.GetBuildIndexByScenePath(levels[index]);

        if (buildIndex < 0)
        {
            Debug.LogError("Scene not found.");
            return;
        }*/

        // unhide Tips hub
        hideTipsSO.hideTips = false;
        loadingLevelText.text = "Loading Level " + (index) + " . . . ";
        AsyncOperation scene = SceneManager.LoadSceneAsync(levels[index-1]);
        StartCoroutine(ProgressScene(scene));
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

    // Load Saved Game
    public void LoadSavedGame()
    {
        Debug.Log("HELLO");
        // hide Tips hub
        hideTipsSO.hideTips = true;
        if (!DataPersistenceManager.instance.HasGameData())
        {
            savedDataNotFoundUI.SetActive(true);
            return;
        }
        Debug.Log("HELLO 2");

        loadingLevelText.text = "Loading Saved Progress . . . ";
        if(DataPersistenceManager.instance == null)
        {
            Debug.LogError("NOTT");
        }
        AsyncOperation scene = SceneManager.LoadSceneAsync(DataPersistenceManager.instance.SavedLevelName());
        StartCoroutine(ProgressScene(scene));
    }

    // Level Load Progress System
    private IEnumerator ProgressScene(AsyncOperation scene)
    {
        sliderUI.value = 0;

        while (!scene.isDone)
        {
            float progress = Mathf.Clamp01(scene.progress / 0.9f);
            sliderUI.value = progress;

            yield return null;
        }

    }



    private void Update()
    {
    }

    public string DoFileExist()
    {
        return Path.Combine(path, fileName);
    }

    // trigger change to either load or set to default
    public void TriggerLoadGame(bool value)
    {
        DataPersistenceManager.instance.SetIsLoad(value);
        hideTipsSO.hideTips = true;
    }

    public void SetLevelSelectIndex(int index)
    {
        DataPersistenceManager.instance.SelectedLevelIndex = index;
    }

}
