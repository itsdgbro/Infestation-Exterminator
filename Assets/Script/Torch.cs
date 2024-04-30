using UnityEngine;

public class Torch : MonoBehaviour
{

    private PlayerControls playerControls;
    private Light torch;

    private void Awake()
    {
        playerControls = new PlayerControls();
        torch = GetComponent<Light>();
    }

    private void Update()
    {
        if (playerControls.Interactive.FlashLight.triggered)
        {
            torch.enabled = !torch.enabled;
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
