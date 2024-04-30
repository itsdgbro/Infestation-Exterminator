using UnityEngine;

public class HideTipsUI : MonoBehaviour
{
    [SerializeField] private GameObject tipsUI;
    [SerializeField] private HideTipsSO hideTipsSO;

    private PlayerControls playerControls;

    // show initial when collided
    // hide tips when true

    private void Awake()
    {
        playerControls = new PlayerControls();
        tipsUI.SetActive(!hideTipsSO.hideTips);
    }

    private void Update()
    {
        ToggleUIShow();
    }

    private void ToggleUIShow()
    {
        if (playerControls.Interactive.ToggleTips.triggered)
        {
            hideTipsSO.hideTips = !hideTipsSO.hideTips;
            tipsUI.SetActive(!hideTipsSO.hideTips);
        }
    }


    #region player controls
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    #endregion
}
