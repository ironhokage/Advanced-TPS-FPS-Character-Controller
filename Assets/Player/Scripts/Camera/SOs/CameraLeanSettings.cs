using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CameraLeanSettings", menuName = "Scriptable Objects/CameraLeanSettings")]
public class CameraLeanSettings : ScriptableObject
{
    [System.Serializable]
    public struct LeanData
    {
        [Tooltip("The name of the state (e.g., 'Slide', 'Default') to match against.")]
        public string stateName;
        [Tooltip("The rotation angle in degrees the camera should lean.")]
        public float leanAngle;
        [Tooltip("How fast the camera interpolates to this lean angle.")]
        public float lerpSpeed;
    }

    [Header("Lean Configuration")]
    [Tooltip("Master toggle to enable or disable all camera leaning logic.")]
    [SerializeField] private bool enableLean = true;

    [Tooltip("List of settings for different movement states. Replaces Godot's Dictionary.")]
    [SerializeField] private List<LeanData> leanStates = new List<LeanData>();

    public bool EnableLean => enableLean;
    public List<LeanData> LeanStates => leanStates;
}

