using UnityEngine;
using UnityEngine.InputSystem;

public class Testing : MonoBehaviour
{

    private RebindUISampleActions playerControls;

    // Start is called before the first frame update
    void Awake()
    {
        playerControls = new RebindUISampleActions();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.Gameplay.Interact.triggered)
        {
            Debug.Log("EE");
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