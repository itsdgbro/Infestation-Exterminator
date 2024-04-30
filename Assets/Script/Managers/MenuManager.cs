using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("Level-SO")]
    [SerializeField] private LevelUnlockSO levelManager;
    [Header("Level GO")]
    [SerializeField] private GameObject[] levelGOs;


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
        AsyncOperation scene = SceneManager.LoadSceneAsync(levels[index - 1]);
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
        // hide Tips hub
        hideTipsSO.hideTips = true;
        if (!DataPersistenceManager.instance.HasGameData())
        {
            savedDataNotFoundUI.SetActive(true);
            return;
        }

        loadingLevelText.text = "Loading Saved Progress . . . ";
        if (DataPersistenceManager.instance == null)
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


    // check if new level is unlocked
    public void CheckLevelUnlocked()
    {
        for (int i = 0; i < levelGOs.Length; i++)
        {
            EventTrigger eventTrigger = levelGOs[i].GetComponent<EventTrigger>();
            Button button = levelGOs[i].GetComponentInChildren<Button>();

            eventTrigger.enabled = false;
            button.enabled = false;

            if (i == 0 && levelManager.isLevel2Unlocked)
            {
                eventTrigger.enabled = true;
                button.enabled = true;
            }
            else if (i == 1 && levelManager.isLevel3Unlocked)
            {
                eventTrigger.enabled = true;
                button.enabled = true;
            }
        }
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
