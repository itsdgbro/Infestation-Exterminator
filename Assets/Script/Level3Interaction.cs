using UnityEngine;

public class Level3Interaction : MonoBehaviour, IDataPersistence
{
    // private PlayerControls playerControls;
    private PlayerInputHandler playerControls;

    [SerializeField] private string id;

    private bool canCollect = false;
    // [SerializeField] private Level3Collections level3Collections;

    // press E to collect
    [SerializeField] private GameObject collectUI;

    // flag to check if collected
    private bool isCollected = false;

    // referenc to parent script
    private ItemCollectedTracker itemCollectedTracker;
    private void Awake()
    {
        id = gameObject.name + gameObject.transform.GetSiblingIndex();
        playerControls = PlayerInputHandler.Instance;
        collectUI.SetActive(false);
        itemCollectedTracker = GetComponentInParent<ItemCollectedTracker>();
        if (itemCollectedTracker == null)
        {
            Debug.LogError("Item Collection Tracker not found");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Can collect");
            canCollect = true;
            collectUI.SetActive(canCollect);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Can not collect");
            canCollect = false;
            collectUI.SetActive(canCollect);
        }
    }

    private void Update()
    {
        if (canCollect && playerControls.Interact)
        {
            gameObject.SetActive(false);
            collectUI.SetActive(false);
            // level3Collections.isCollected[transform.GetSiblingIndex()] = true;
            isCollected = true;
            itemCollectedTracker.SetItemCollectCount(this.gameObject);
            Destroy(this.gameObject, 15.0f);
        }
    }

    public void LoadData(GameData data)
    {
        if (data.itemCollected.isItemCollected.ContainsKey(id))
        {
            this.isCollected = data.itemCollected.isItemCollected[id];
        }
        else
        {
            this.isCollected = true && DataPersistenceManager.instance.GetIsLoad();
        }
        if (isCollected)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    public void SaveData(GameData data)
    {

        if (data.itemCollected.isItemCollected.ContainsKey(id))
        {
            data.itemCollected.isItemCollected.Remove(id);
        }

        data.itemCollected.isItemCollected.Add(id, isCollected);
    }
}
