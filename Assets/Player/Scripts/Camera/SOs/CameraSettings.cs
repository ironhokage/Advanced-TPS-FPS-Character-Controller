using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Scriptable Objects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    [System.Serializable]
    public struct FovStateData
    {
        [Tooltip("The name of the state (e.g., 'Run', 'Dash').")]
        public string stateName;
        [Tooltip("The Field of View value for this state.")]
        public float fovValue;
        [Tooltip("How long (in seconds) it takes to transition to this FOV.")]
        public float transitionDuration;
    }

    [Header("Base Rotation & FOV")]
    [Tooltip("The default base Field of View.")]
    [Range(5.0f, 175.0f)]
    [SerializeField] private float defaultFov = 90.0f;

    [Header("State-Based FOV Configuration")]
    [Tooltip("Configuration for FOV changes per movement state. Replaces Godot's Dictionary.")]
    [SerializeField] private List<FovStateData> stateFovSettings = new List<FovStateData>();

    // Getters for implementation
    public float DefaultFov => defaultFov;
    public List<FovStateData> StateFovSettings => stateFovSettings;
}
