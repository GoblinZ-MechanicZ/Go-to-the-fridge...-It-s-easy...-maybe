using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GlobalSettingsSliderItem : MonoBehaviour
{
    [SerializeField] private GlobalSettingsScriptable globalSettings;
    [SerializeField] private AudioMixerGroup masterGroup;

    [SerializeField] private bool isBrightness;
    [SerializeField] private bool isSound;

    [SerializeField] private Slider slider;

    private void OnEnable()
    {
        slider.value =
                (isBrightness) ? globalSettings.brightness :
                    (isSound) ? globalSettings.masterVolume :
                        0.5f;
    }

    public void Reset()
    {
        slider.value =
                (isBrightness) ? globalSettings.brightness :
                    (isSound) ? globalSettings.masterVolume :
                        0.5f;
    }

    public void OnChangeBrightness(float newValue)
    {
        globalSettings.brightness = newValue;
    }

    public void OnChangeSound(float newValue)
    {
        masterGroup.audioMixer.SetFloat("MasterVolume", newValue);
        globalSettings.masterVolume = newValue;
    }
}