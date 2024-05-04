using UnityEngine;

public class Torch : MonoBehaviour
{

    // private PlayerControls playerControls;
    private PlayerInputHandler playerControls;

    private Light torch;

    private void Awake()
    {
        playerControls = PlayerInputHandler.Instance;
        torch = GetComponent<Light>();
    }

    private void Update()
    {
        if (playerControls.FlashLight)
        {
            torch.enabled = !torch.enabled;
        }
    }
}
