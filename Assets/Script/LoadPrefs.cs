using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class LoadPrefs : MonoBehaviour
{

    [Header("Post-Processing-Brightness Header")]
    public PostProcessProfile brightnessExposure;
    public PostProcessLayer postLayer;
    private AutoExposure autoExposure;

    [Header("Load Audio Prefs")]
    public AudioMixer audioMixer;

    private readonly string brightnessPref = "Brightness";

    private readonly string masterVolumePref = "MasterVolume";
    private readonly string musicVolumePref = "MusicVolume";
    private readonly string sfxVolumePref = "SFXVolume";

    void Start()
    {
        // get exposure
        brightnessExposure.TryGetSettings(out autoExposure);

        // load brightness pref
        float brightnessValue = PlayerPrefs.HasKey(brightnessPref) ? PlayerPrefs.GetFloat(brightnessPref) : 0.5f;

        autoExposure.keyValue.value = brightnessValue * 2;

        // load audio
        float masterVolume = PlayerPrefs.HasKey(masterVolumePref) ? PlayerPrefs.GetFloat(masterVolumePref) : 0f;
        audioMixer.SetFloat("master", masterVolume);

        float musicVolume = PlayerPrefs.HasKey(musicVolumePref) ? PlayerPrefs.GetFloat(musicVolumePref) : 0.5f;
        audioMixer.SetFloat("music", musicVolume);
    }
}
