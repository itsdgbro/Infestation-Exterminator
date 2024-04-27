using Unity.VisualScripting;
using UnityEngine;

public class Level3Interaction : MonoBehaviour
{

    private BoxCollider boxCollider;
    private PlayerControls playerControls;
    private bool canCollect = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
        boxCollider = GetComponent<BoxCollider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Can collect");
            canCollect = true;
        }
        else
        {
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Can not collect");
            canCollect = false;
        }
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     // if (other.gameObject.CompareTag("Player"))
    //     // {
    //     //     Debug.Log("Collected 12");
    //     //     // Press E to evacuate 
    //     //     if (playerControls.Interactive.Interact.triggered)
    //     //     {
    //     //         Debug.Log("Collected");
    //     //     }
    //     // }
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         Debug.Log("Collected");
    //     }
    // }

    private void Update()
    {
        if (canCollect && playerControls.Interactive.Interact.triggered)
        {
            Debug.Log(gameObject.name + " Collected");
            gameObject.SetActive(false);
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
