using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCollectedTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI collectTextUI;
    private List<GameObject> collectedList = new();

    private int totalItems;
    private int iItemCollectCount;

    public int GetItemCollectCount() => iItemCollectCount;

    public void SetItemCollectCount(GameObject gameObject)
    {
        collectedList.Remove(gameObject);
        ItemCollectedUI();
    }

    private void Start()
    {
        totalItems = transform.childCount;
        AppendCollectedList();
        ItemCollectedUI();
    }

    private void AppendCollectedList()
    {
        collectedList.Clear();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                collectedList.Add(child.gameObject);
            }
        }
        if (collectedList.Count == 0)
        {
            iItemCollectCount = 3;
        }
    }

    private void ItemCollectedUI()
    {
        iItemCollectCount = totalItems - collectedList.Count;
        collectTextUI.text = iItemCollectCount.ToString() + "/" + totalItems.ToString();
    }

}
