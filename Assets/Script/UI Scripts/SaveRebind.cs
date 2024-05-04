using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class SaveRebind : MonoBehaviour
{

    public InputActionAsset actions;
    public Slider mouseSensiSlider;
    public TextMeshProUGUI mouseSensiValueDisplay;

    private string rebindString = "rebinds";
    private string mouseSensiFloat = "MouseSensitivity";

    private float mouseSensi;

    public void SaveNewControls()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(rebindString, rebinds);
        PlayerPrefs.SetFloat(mouseSensiFloat, mouseSensi);
    }

    void Update()
    {
        mouseSensi = mouseSensiSlider.value;
        mouseSensiValueDisplay.text = mouseSensi.ToString("F2");
    }

    public void SetMouseSensitivity()
    {
        PlayerPrefs.SetFloat(mouseSensiFloat, mouseSensi);
    }

    public void OnEnable()
    {

        var rebinds = PlayerPrefs.GetString(rebindString);
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);

        mouseSensi = PlayerPrefs.GetFloat(mouseSensiFloat);
        mouseSensiSlider.value = mouseSensi;
        mouseSensiValueDisplay.text = mouseSensi.ToString("F2");
    }
}
