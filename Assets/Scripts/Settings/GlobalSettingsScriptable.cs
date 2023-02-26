using UnityEngine;

[CreateAssetMenu(fileName = "GlobalSettingsScriptable", menuName = "Varvar/GlobalSettingsScriptable", order = 0)]
public class GlobalSettingsScriptable : ScriptableObject
{
    public float masterVolume = 100f;
    public float brightness = 50f;

    [Space()]
    [Header("Stress")]
    public float heartVolumeReduction = 3f;
    public float panicMoves = 15f;
    public float stressChillTime = 15f;

    public bool panicAtMaxStressLevel = true;
    public bool turnOffTorchWhenPanic = true;

    public float maxStressLevel = 100f;
    public float stressPerSecond = 0.16f;
    public float stressChangeWhenLoseJar = 100f;
    public float stressAfterPanic = 50f;
    public float stressChangeWhenTakeBonus = 0f;
    public float stressModWhenEnemyInRoom = 1f;
    public float stressWhenBatFlyAround = 10f;

}