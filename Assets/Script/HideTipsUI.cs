using UnityEngine;

public class HideTipsUI : MonoBehaviour
{
    // private PlayerControls playerControls;
    private PlayerInputHandler playerControls;

    [SerializeField] private GameObject tipsUI;
    [SerializeField] private HideTipsSO hideTipsSO;

    // show initial when collided
    // hide tips when true

    private void Awake()
    {
        playerControls = PlayerInputHandler.Instance;
        tipsUI.SetActive(!hideTipsSO.hideTips);
    }

    private void Update()
    {
        ToggleUIShow();
    }

    private void ToggleUIShow()
    {
        if (playerControls.ToggleTips)
        {
            hideTipsSO.hideTips = !hideTipsSO.hideTips;
            tipsUI.SetActive(!hideTipsSO.hideTips);
        }
    }
}
