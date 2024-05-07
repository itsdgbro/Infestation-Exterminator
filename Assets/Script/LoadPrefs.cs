using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LoadPrefs : MonoBehaviour
{

    [Header("Post-Processing-Brightness Header")]
    public PostProcessProfile brightnessExposure;
    public PostProcessLayer postLayer;
    private AutoExposure autoExposure;

    private readonly string brightnessPref = "Brightness";

    void Start()
    {
        // get exposure
        brightnessExposure.TryGetSettings(out autoExposure);

        // load brightness pref
        float brightnessValue = PlayerPrefs.HasKey(brightnessPref) ? PlayerPrefs.GetFloat(brightnessPref) : 0.5f;

        autoExposure.keyValue.value = brightnessValue * 2;

    }
}
