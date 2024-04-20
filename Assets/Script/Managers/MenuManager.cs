using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
        fileName = DataPersistenceManager.instance.GetFileName();
        sliderUI.value = 0f;
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
        hideTipsSO.hideTips = false;
        loadingLevelText.text = "Loading Level " + (index + 1) + " . . . ";
        AsyncOperation scene = SceneManager.LoadSceneAsync(levels[index]);
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
        AsyncOperation scene = SceneManager.LoadSceneAsync(DataPersistenceManager.instance.SavedLevelName());
        StartCoroutine(ProgressScene(scene));
    }

    /* private IEnumerator ProgressScene()
     {
         sliderUI.value = 0;
         progressValue = 0f;
         scene.allowSceneActivation = false;
         while (scene.progress < 0.9f)
         {
             // Update the slider's value
             progressValue = scene.progress;
             Debug.Log(progressValue + " asd");
             yield return null;
         }
         scene.allowSceneActivation = true;
     }*/

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
