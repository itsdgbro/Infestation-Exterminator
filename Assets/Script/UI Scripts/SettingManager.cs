using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("Graphic Setting References")]
    public Slider brightnessSlider;
    public TextMeshProUGUI brightnessValueDisplay;
    public Toggle fullScreenTog;
    public Toggle vsyncTog;
    public TMP_Dropdown resDropDown;

    Resolution[] allResolutions;
    int selectedResolution;

    List<Resolution> selectedResolutionList = new List<Resolution>();



    [Header("Control Setting References")]
    public InputActionAsset actions;
    public Slider mouseSensiSlider;
    public TextMeshProUGUI mouseSensiValueDisplay;

    // PlayerPrefs references
    private readonly string brightnessPref = "Brightness";
    private readonly string resolutionPref = "SavedResolution";
    private readonly string vsyncPref = "SavedVsync";

    private readonly string rebindStringPref = "rebinds";
    private readonly string mouseSensiFloatPRef = "MouseSensitivity";
    private float brightnessValue;
    private float mouseSensi;

    private void Awake()
    {
        fullScreenTog.isOn = Screen.fullScreen;
        vsyncTog.isOn = QualitySettings.vSyncCount != 0;

        allResolutions = Screen.resolutions;

        List<string> resListString = new List<string>();

        string newRes;
        foreach (Resolution res in allResolutions)
        {
            // Check if the aspect ratio is 16:9 or 16:10
            float aspectRatio = (float)res.width / res.height;
            if (Mathf.Approximately(aspectRatio, 16f / 9f) || Mathf.Approximately(aspectRatio, 16f / 10f))
            {
                newRes = res.width.ToString() + " x " + res.height.ToString();

                if (!resListString.Contains(newRes))
                {
                    resListString.Add(newRes);
                    selectedResolutionList.Add(res);
                }
            }
        }
        resDropDown.AddOptions(resListString);

        // Check if the key exists in PlayerPrefs
        selectedResolution = PlayerPrefs.HasKey(resolutionPref) ? PlayerPrefs.GetInt(resolutionPref) : selectedResolutionList.Count - 1;
        brightnessSlider.value = Mathf.Floor((PlayerPrefs.HasKey(brightnessPref) ? PlayerPrefs.GetFloat(brightnessPref) : 0.5f) * 100f);
        string formattedValue = brightnessSlider.value.ToString("F0"); // Format as a string with 0 decimal places
        brightnessSlider.value = float.Parse(formattedValue); // Parse back to float

        resDropDown.value = selectedResolution;
    }

    private void ChangeResolution(int value)
    {
        Screen.SetResolution(selectedResolutionList[value].width, selectedResolutionList[value].height, fullScreenTog.isOn);
        PlayerPrefs.SetInt(resolutionPref, value);
    }

    public void SaveNewGraphics()
    {

        selectedResolution = resDropDown.value;
        Screen.fullScreen = fullScreenTog.isOn;
        int isVsyncOn = vsyncTog.isOn ? 1 : 0;
        QualitySettings.vSyncCount = isVsyncOn;
        PlayerPrefs.SetInt(vsyncPref, isVsyncOn);
        PlayerPrefs.SetFloat(vsyncPref, brightnessSlider.value);
        brightnessSlider.value = Mathf.Floor((PlayerPrefs.HasKey(brightnessPref) ? PlayerPrefs.GetFloat(brightnessPref) : 0.5f) * 100f);
        string formattedValue = brightnessSlider.value.ToString("F0"); // Format as a string with 0 decimal places
        brightnessSlider.value = float.Parse(formattedValue); // Parse back to float

        ChangeResolution(selectedResolution);
    }

    public void ResetGraphicSettings()
    {
        selectedResolution = selectedResolutionList.Count - 1;
        PlayerPrefs.SetInt(resolutionPref, selectedResolution);
        Screen.fullScreen = true;
        QualitySettings.vSyncCount = 1;
        brightnessSlider.value = 0.5f;
        PlayerPrefs.SetFloat(vsyncPref, brightnessSlider.value);
        ChangeResolution(selectedResolution);
    }

    // Control settings
    public void SaveNewControls()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(rebindStringPref, rebinds);
        PlayerPrefs.SetFloat(mouseSensiFloatPRef, mouseSensi);
    }

    void Update()
    {
        brightnessValue = brightnessSlider.value;
        brightnessValueDisplay.text = brightnessValue.ToString("F2");

        mouseSensi = mouseSensiSlider.value;
        mouseSensiValueDisplay.text = mouseSensi.ToString("F2");
    }

    public void SetMouseSensitivity()
    {
        PlayerPrefs.SetFloat(mouseSensiFloatPRef, mouseSensi);
    }

    public void OnEnable()
    {

        var rebinds = PlayerPrefs.GetString(rebindStringPref);
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);

        mouseSensi = PlayerPrefs.GetFloat(mouseSensiFloatPRef);
        mouseSensiSlider.value = mouseSensi;
        mouseSensiValueDisplay.text = mouseSensi.ToString("F2");
    }
}
