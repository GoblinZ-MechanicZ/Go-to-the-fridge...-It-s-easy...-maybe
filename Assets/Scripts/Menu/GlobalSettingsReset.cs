using UnityEngine;

public class GlobalSettingsReset : MonoBehaviour
{
    [SerializeField] private GlobalSettingsScriptable globalSettings;
    [Range(-20, 0)]
    [SerializeField] private float defaultSoundValue = 0f;
    [Range(0, 100)]
    [SerializeField] private float defaultBrightnessValue = 50f;
    [SerializeField] private UnityEngine.Audio.AudioMixer masterMixer;

    public void ResetSound()
    {
        globalSettings.masterVolume = defaultSoundValue;
        masterMixer.SetFloat("MasterVolume", defaultSoundValue);
    }

    public void ResetBrightness()
    {
        globalSettings.brightness = defaultBrightnessValue;
    }

    public void ResetAll()
    {
        ResetSound();
        ResetBrightness();
    }
}