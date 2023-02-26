using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettingsScriptable", menuName = "Varvar/GlobalSettingsScriptable", order = 0)]
public class GlobalSettingsScriptable : ScriptableObject {
    public float masterVolume = 100f;
    public float brightness = 50f;
}