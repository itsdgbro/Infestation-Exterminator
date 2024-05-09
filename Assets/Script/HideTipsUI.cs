using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideTipsUI : MonoBehaviour
{

    [SerializeField] private GameObject tipsUI;
    [SerializeField] private HideTipsSO hideTipsSO;

    private void Awake()
    {
        tipsUI.SetActive(!hideTipsSO.hideTips);
    }

    private void Start()
    {
        PlayerInputHandler.Instance.ToggleTipsAction.started += ToggleUIShow;
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
        PlayerInputHandler.Instance.ToggleTipsAction.started -= ToggleUIShow;
    }

}
