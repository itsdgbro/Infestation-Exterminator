using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideTipsUI : MonoBehaviour
{

    [SerializeField] private GameObject tipsUI;
    [SerializeField] private HideTipsSO hideTipsSO;

    // // Input Action
    private InputAction inputAction;

    private void Awake()
    {
        tipsUI.SetActive(!hideTipsSO.hideTips);
        inputAction = PlayerInputHandler.Instance.ToggleTipsAction;

        // Action Subscribe
        inputAction.started += ToggleUIShow;
    }

    private void ToggleUIShow(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            hideTipsSO.hideTips = !hideTipsSO.hideTips;
            tipsUI.SetActive(!hideTipsSO.hideTips);
        }
    }

    private void OnDisable()
    {
        // Action Unsubscribe
        inputAction.started -= ToggleUIShow;
    }


}
