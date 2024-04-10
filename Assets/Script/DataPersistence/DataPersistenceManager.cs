using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public GameData gameData { get; set; }
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler fileDataHandler;
    public static DataPersistenceManager instance { get; private set; }

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
        this.gameData = new GameData();
    }


    // Save game progress
    //public void SaveGame()
    //{
    //    if(gameData == null)
    //    {
    //        Debug.Log("NULL");
    //        this.gameData = new GameData();
    //    }

    //    // pass the data to other scripts so they can update it
    //    foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
    //    {   
    //        Debug.Log("Not Found");
    //        dataPersistence.SaveData(gameData);
    //    }

    //    // save the data to file
    //    fileDataHandler.Save(gameData);
    //}
    public void SaveGame()
    {   
        if(gameData == null)
        {
            this.gameData = new GameData();
        }

        if(dataPersistenceObjects == null)
        {
            Debug.LogWarning("DPo Not Found");
            return;
        }
        // pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistence in dataPersistenceObjects)
        {
            Debug.Log(dataPersistence);
            dataPersistence.SaveData(gameData);
        }

        // save the data to file
        fileDataHandler.Save(gameData);
    }

    // Load game from saved data
    public void LoadGame()
    {
        // load any saved data from a file using the data handler
        this.gameData = fileDataHandler.Load();

        // if no data can be loaded, don't continue
        if (this.gameData == null || string.IsNullOrEmpty(this.gameData.sceneName))
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
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

}
