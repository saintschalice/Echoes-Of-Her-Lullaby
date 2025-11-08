using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SentenceTimestamp
{
    [TextArea(2, 4)]
    public string sentence; // The full sentence text
    public float startTime; // When this sentence should start appearing (in seconds from audio start)

    [Tooltip("Optional: When this sentence should disappear. Leave at 0 to keep visible until line fades out.")]
    public float endTime = 0f; // Optional end time for this sentence (0 = keep until line ends)

    [Tooltip("Fade in duration for this sentence")]
    public float fadeInDuration = 0.3f; // How long the fade in takes
}

[System.Serializable]
public class CutsceneLine
{
    [TextArea(2, 4)]
    public string fullText; // Full line text (for reference only)
    public List<SentenceTimestamp> sentences = new List<SentenceTimestamp>();
    public float fadeOutDuration = 0.5f; // Fade out duration after line completes
    public float pauseAfterLine = 0.5f; // Additional pause after fade out
}

[CreateAssetMenu(fileName = "NewCutscene", menuName = "Narrative/VoiceOver Cutscene Data")]
public class VoiceOverCutsceneData : ScriptableObject
{
    [Header("Audio")]
    public AudioClip voiceOverAudio; // Main voiceover/dialogue
    public AudioClip backgroundMusic; // Optional music to play during cutscene

    [Range(0f, 1f)]
    public float voiceoverVolume = 1f;

    [Range(0f, 1f)]
    public float musicVolume = 0.3f; // Lower by default so it doesn't overpower voiceover

    [Header("Cutscene Lines")]
    public List<CutsceneLine> lines = new List<CutsceneLine>();

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public float sentenceFadeInDuration = 0.3f; // Fade in duration for each sentence
    public Color backgroundColor = Color.black;
}