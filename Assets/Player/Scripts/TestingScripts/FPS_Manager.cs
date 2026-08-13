using UnityEngine;
using System.Collections;

public class FPSLimiter : MonoBehaviour
{
    [Header("Main Settings")]
    public bool useVSync = false;
    [Range(10, 240)] public int targetFPS = 60;
    
    [Header("Performance Tests")]
    public bool powerSaveInBackground = true;
    public bool simulateCPUSpike = false;

    private int _lastFrameRate;
    private bool _lastVSyncState;

    void Awake() => StartCoroutine(SafeInit());

    IEnumerator SafeInit()
    {
        // Wait for buffer stability to prevent the "upside down" bug
        yield return new WaitForEndOfFrame();
        ApplySettings();
    }

    void Update()
    {
        // Auto-update if settings change in Inspector
        if (targetFPS != _lastFrameRate || useVSync != _lastVSyncState)
        {
            ApplySettings();
        }

        // TEST: Press 'S' to simulate a frame spike (CPU Lag)
        if (simulateCPUSpike)
        {
            simulateCPUSpike = false;
            SimulateLoad(100); 
        }
    }

    void ApplySettings()
    {
        // VSync logic: 0 = Off, 1 = On (Syncs to Monitor Refresh Rate)
        QualitySettings.vSyncCount = useVSync ? 1 : 0;

        // If VSync is ON, targetFrameRate is ignored by the engine
        Application.targetFrameRate = useVSync ? -1 : targetFPS;

        _lastFrameRate = targetFPS;
        _lastVSyncState = useVSync;

        string mode = useVSync ? "VSync ON (Monitor Managed)" : $"VSync OFF (Capped at {targetFPS})";
        Debug.Log($"<color=cyan>Graphics Settings Applied:</color> {mode}");
    }

    void SimulateLoad(int ms)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < ms) { /* Waste Cycles */ }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (powerSaveInBackground && !useVSync)
        {
            Application.targetFrameRate = hasFocus ? targetFPS : 30;
        }
    }
}