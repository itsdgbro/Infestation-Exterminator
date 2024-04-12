using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public string GetFileName() => fileName;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler fileDataHandler;

    public static DataPersistenceManager instance { get; private set; }

    [SerializeField] private bool isLoad = false;

    public void SetIsLoad(bool value)
    {
        isLoad = value;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene, destroying newest one");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    // scene transition settings
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void NewGame()
    {
        Debug.Log("New game started. Resetting game data...");
        this.gameData = new GameData();
    }


    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.LogWarning("Data not found");
            return;
        }

        if (dataPersistenceObjects == null)
        {
            Debug.LogWarning("DPo Not Found");
            return;
        }
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(gameData);
        }

        // save the data to file
        fileDataHandler.Save(gameData);
    }

    // Load game from saved data
    public void LoadGame()
    {
        if (isLoad)
        {
            // load any saved data from a file using the data handler
            this.gameData = fileDataHandler.Load();
            // if no data can be loaded, don't continue
            if (this.gameData == null)
            {
                Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
                NewGame();
            }
        }
        else
        {
            NewGame();
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }


    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    // Expose fileDataHandler through a method
    public FileDataHandler GetFileDataHandler()
    {
        return fileDataHandler;
    }

    // Expose dataPersistenceObjects through a method
    public List<IDataPersistence> GetDataPersistenceObjects()
    {
        return dataPersistenceObjects;
    }


    public bool HasGameData()
    {
        return gameData != null;
    }

    public string SavedLevelName()
    {
        return gameData.sceneName;
    }
}
