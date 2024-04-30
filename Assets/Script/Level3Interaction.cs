using Unity.VisualScripting;
using UnityEngine;

public class Level3Interaction : MonoBehaviour
{

    private PlayerControls playerControls;
    private bool canCollect = false;
    [SerializeField] private Level3Collections level3Collections;
    [SerializeField] private GameObject collectUI;

    // referenc to parent script
    private ItemCollectedTracker itemCollectedTracker;
    private void Awake()
    {
        playerControls = new PlayerControls();
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
        if (canCollect && playerControls.Interactive.Interact.triggered)
        {
            Debug.Log(gameObject.name + " Collected");
            gameObject.SetActive(false);
            collectUI.SetActive(false);
            level3Collections.isCollected[transform.GetSiblingIndex()] = true;
            itemCollectedTracker.ItemCollectedCounter();

        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
