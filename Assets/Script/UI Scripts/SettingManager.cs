using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [Header("Graphic Setting References")]
    public Slider brightnessSlider;
    public TextMeshProUGUI brightnessValueDisplay;
    public Toggle fullScreenTog;
    public Toggle vsyncTog;
    public TMP_Dropdown resDropDown;
    public TMP_Dropdown qualityDropDown;

    Resolution[] allResolutions;
    int selectedResolution;
    List<Resolution> selectedResolutionList = new List<Resolution>();

    private int selectedQuality;

    [Header("Post-Processing-Brightness Header")]
    public PostProcessProfile brightnessExposure;
    public PostProcessLayer postLayer;

    [Header("Control Setting References")]
    public InputActionAsset actions;
    public Slider mouseSensiSlider;
    public TextMeshProUGUI mouseSensiValueDisplay;

    // PlayerPrefs references
    private readonly string brightnessPref = "Brightness";
    private readonly string resolutionPref = "SavedResolution";
    private readonly string graphicPref = "SavedGraphic";
    private readonly string vsyncPref = "SavedVsync";

    private readonly string rebindStringPref = "rebinds";
    private readonly string mouseSensiFloatPRef = "MouseSensitivity";

    private readonly string masterVolumePref = "MasterVolume";
    private readonly string musicVolumePref = "MusicVolume";
    private readonly string sfxVolumePref = "SFXVolume";

    private float brightnessValue;
    private float mouseSensi;

    private AutoExposure exposure;  // brightness

    [Header("Audio Setting Preferences")]
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private float masterValue;
    private float musicValue;
    private float sfxValue;

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
            if (Mathf.Approximately(aspectRatio, 16f / 9f)) // 16:9 display
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
        if (selectedResolution > selectedResolutionList.Count)
        {
            selectedResolution = selectedResolutionList.Count - 1;
        }
        selectedQuality = PlayerPrefs.HasKey(graphicPref) ? PlayerPrefs.GetInt(graphicPref) : qualityDropDown.options.Count - 2;

        // get exposure
        brightnessExposure.TryGetSettings(out exposure);

        // load brightness
        brightnessValue = PlayerPrefs.HasKey(brightnessPref) ? PlayerPrefs.GetFloat(brightnessPref) : 0.5f;
        brightnessSlider.value = brightnessValue;   // slider
        brightnessValueDisplay.text = (brightnessValue * 100f).ToString("F0") + "%"; // Format as a string with 0 decimal places

        exposure.keyValue.value = brightnessValue * 2;

        // resolution
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, fullScreenTog.isOn);
        resDropDown.value = selectedResolution;

        // graphic
        QualitySettings.SetQualityLevel(selectedQuality);
        qualityDropDown.value = selectedQuality;

        // audio
        masterValue = PlayerPrefs.HasKey(masterVolumePref) ? PlayerPrefs.GetFloat(masterVolumePref) : 0f;
        masterSlider.value = masterValue;
        audioMixer.SetFloat("master", masterValue);

        musicValue = PlayerPrefs.HasKey(musicVolumePref) ? PlayerPrefs.GetFloat(musicVolumePref) : 0.5f;
        musicSlider.value = musicValue;
        audioMixer.SetFloat("audio", musicValue);

        sfxValue = PlayerPrefs.HasKey(sfxVolumePref) ? PlayerPrefs.GetFloat(sfxVolumePref) : 0f;
        sfxSlider.value = sfxValue;
        audioMixer.SetFloat("sfx", sfxValue);
    }

    // set brightness
    public void ChangeBrightness(float value)
    {
        brightnessValue = value;
        exposure.keyValue.value = value * 2;
        brightnessValueDisplay.text = (brightnessValue * 100).ToString("F0") + "%";
    }

    // set resolution
    private void ChangeResolution(int value)
    {
        Screen.SetResolution(selectedResolutionList[value].width, selectedResolutionList[value].height, fullScreenTog.isOn);
        PlayerPrefs.SetInt(resolutionPref, value);
    }

    // set graphic quality
    public void ChangeGraphicQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt(graphicPref, index);
    }

    // save graphic component
    public void SaveNewGraphics()
    {

        PlayerPrefs.SetFloat(brightnessPref, brightnessValue);

        // res and quality
        selectedResolution = resDropDown.value;
        selectedQuality = qualityDropDown.value;

        ChangeResolution(selectedResolution);
        ChangeGraphicQuality(selectedQuality);

        // full screen / vsync
        Screen.fullScreen = fullScreenTog.isOn;
        int isVsyncOn = vsyncTog.isOn ? 1 : 0;
        QualitySettings.vSyncCount = isVsyncOn;
        PlayerPrefs.SetInt(vsyncPref, isVsyncOn);

    }


    // reset graphic component
    public void ResetGraphicSettings()
    {
        brightnessValue = 0.5f;
        ChangeBrightness(brightnessValue);

        selectedResolution = selectedResolutionList.Count - 1;
        selectedQuality = qualityDropDown.options.Count - 2;    // second hightest 

        resDropDown.value = selectedResolution;
        qualityDropDown.value = selectedQuality;

        PlayerPrefs.SetInt(resolutionPref, selectedResolution);
        PlayerPrefs.SetInt(graphicPref, selectedQuality);

        ChangeResolution(selectedResolution);
        ChangeGraphicQuality(selectedQuality);

        Screen.fullScreen = true;
        fullScreenTog.isOn = true;

        QualitySettings.vSyncCount = 1;
        vsyncTog.isOn = true;

        brightnessSlider.value = 0.5f;
        PlayerPrefs.SetFloat(vsyncPref, brightnessSlider.value);
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

    // audio
    public void SetMaterVolume(float sliderValue)
    {
        masterValue = sliderValue;
        audioMixer.SetFloat("master", masterValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicValue = sliderValue;
        musicValue = musicValue == -20f ? -80 : musicValue; // mute at lowest
        audioMixer.SetFloat("music", musicValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxValue = sliderValue;
        audioMixer.SetFloat("sfx", sfxValue);
    }

    public void SaveNewVolume()
    {
        // master volume
        PlayerPrefs.SetFloat(masterVolumePref, masterValue);

        // music volume
        PlayerPrefs.SetFloat(musicVolumePref, musicValue);

        // sfx volume
        PlayerPrefs.SetFloat(sfxVolumePref, sfxValue);

    }

    public void ResetAudioSetting()
    {
        masterValue = 0f;
        musicValue = 0.5f;
        sfxValue = 0f;

        audioMixer.SetFloat("master", masterValue);
        audioMixer.SetFloat("music", musicValue);
        audioMixer.SetFloat("sfx", sfxValue);

        masterSlider.value = masterValue;
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;

        SaveNewVolume();
    }
}
