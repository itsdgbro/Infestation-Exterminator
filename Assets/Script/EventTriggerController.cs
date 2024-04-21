using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventTriggerController : MonoBehaviour
{
    private EventTrigger eventTrigger;
    private Button button;

    private void Awake()
    {
        eventTrigger = GetComponent<EventTrigger>();
        eventTrigger.enabled = false;
        
        button = GetComponentInChildren<Button>();
        if(button == null)
        {
            Debug.LogError("Button Not Found");
        }
        button.enabled = false;
    }

}
