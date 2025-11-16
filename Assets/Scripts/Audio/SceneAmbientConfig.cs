using UnityEngine;

[CreateAssetMenu(fileName = "AmbientConfig_", menuName = "Audio/Scene Ambient Config", order = 1)]
public class SceneAmbientConfig : ScriptableObject
{
    [Tooltip("The seamless looping clip for this scene.")]
    public AudioClip ambientClip;

    [Range(0.1f, 5.0f)]
    public float fadeTime = 1.0f;

    [Range(0f, 1f)]
    [Tooltip("Volume multiplier specific to this clip (on top of the master ambient volume).")]
    public float clipVolume = 1f;
}