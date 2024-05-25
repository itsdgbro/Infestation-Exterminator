using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Torch : MonoBehaviour
{

    private Light torch;

    private void Awake()
    {
        torch = GetComponent<Light>();
    }

    void Start()
    {
        // Action Subscription
        PlayerInputHandler.Instance.FlashLightAction.started += ToggleTorchLight;
    }

    private void ToggleTorchLight(InputAction.CallbackContext context)
    {
        if (context.started)
            torch.enabled = !torch.enabled;
    }

    void OnDisable()
    {
        // Action unsubscription
        PlayerInputHandler.Instance.FlashLightAction.started -= ToggleTorchLight;
    }
}
