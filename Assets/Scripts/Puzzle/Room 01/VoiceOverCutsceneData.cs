using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WordTimestamp
{
    public string word;
    public float startTime; // When this word should appear (in seconds from audio start)
}

[System.Serializable]
public class CutsceneLine
{
    [TextArea(2, 4)]
    public string fullText; // Full line text (for reference)
    public List<WordTimestamp> words = new List<WordTimestamp>();
    public float pauseAfterLine = 0f; // Pause duration after this line completes
}

[CreateAssetMenu(fileName = "NewCutscene", menuName = "Narrative/VoiceOver Cutscene Data")]
public class VoiceOverCutsceneData : ScriptableObject
{
    [Header("Audio")]
    public AudioClip voiceOverAudio;

    [Header("Cutscene Lines")]
    public List<CutsceneLine> lines = new List<CutsceneLine>();

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public Color backgroundColor = Color.black;
}