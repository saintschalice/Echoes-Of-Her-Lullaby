using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
[CustomEditor(typeof(VoiceOverCutsceneData))]
public class VoiceOverCutsceneDataEditor : Editor
{
    private string newSentenceText = "";
    private float sentenceInterval = 3f; // Default time between sentences
    private float fadeInDuration = 0.3f; // Default fade in duration

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VoiceOverCutsceneData data = (VoiceOverCutsceneData)target;

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Quick Sentence Creation Tool", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Add a sentence with auto-generated timestamp. Adjust timestamps manually after recording your voiceover.", MessageType.Info);

        newSentenceText = EditorGUILayout.TextArea(newSentenceText, GUILayout.Height(60));
        sentenceInterval = EditorGUILayout.Slider("Time Between Sentences (seconds)", sentenceInterval, 0.5f, 10f);
        fadeInDuration = EditorGUILayout.Slider("Fade In Duration (seconds)", fadeInDuration, 0.1f, 2f);

        if (GUILayout.Button("Add Sentence to Current Line", GUILayout.Height(30)))
        {
            AddSentenceToLine(data);
        }

        if (GUILayout.Button("Add Sentence as New Line", GUILayout.Height(30)))
        {
            AddSentenceAsNewLine(data);
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Clear All Lines", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("Clear All Lines", "Are you sure you want to delete all cutscene lines?", "Yes", "No"))
            {
                data.lines.Clear();
                EditorUtility.SetDirty(data);
            }
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Manual Timestamp Helper", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Play the audio and note timestamps for each sentence, then manually enter them in the lines above.", MessageType.Info);

        if (data.voiceOverAudio != null)
        {
            EditorGUILayout.LabelField($"Audio Duration: {data.voiceOverAudio.length:F2} seconds");
        }
    }

    void AddSentenceToLine(VoiceOverCutsceneData data)
    {
        if (string.IsNullOrWhiteSpace(newSentenceText))
        {
            EditorUtility.DisplayDialog("Error", "Please enter some text first!", "OK");
            return;
        }

        // Get or create last line
        CutsceneLine line;
        if (data.lines.Count == 0)
        {
            line = new CutsceneLine();
            line.sentences = new List<SentenceTimestamp>();
            line.fadeOutDuration = 0.5f;
            line.pauseAfterLine = 0.5f;
            data.lines.Add(line);
        }
        else
        {
            line = data.lines[data.lines.Count - 1];
        }

        // Calculate starting time
        float startTime = 0f;
        if (line.sentences.Count > 0)
        {
            SentenceTimestamp lastSentence = line.sentences[line.sentences.Count - 1];
            startTime = lastSentence.startTime + sentenceInterval;
        }
        else if (data.lines.Count > 1)
        {
            // Get time from previous line
            CutsceneLine prevLine = data.lines[data.lines.Count - 2];
            if (prevLine.sentences.Count > 0)
            {
                SentenceTimestamp lastSentence = prevLine.sentences[prevLine.sentences.Count - 1];
                startTime = lastSentence.startTime + prevLine.fadeOutDuration + prevLine.pauseAfterLine;
            }
        }

        // Create sentence
        SentenceTimestamp sentence = new SentenceTimestamp
        {
            sentence = newSentenceText,
            startTime = startTime,
            endTime = 0f, // Optional
            fadeInDuration = fadeInDuration
        };

        line.sentences.Add(sentence);
        EditorUtility.SetDirty(data);
        newSentenceText = "";

        Debug.Log($"Added sentence at {startTime:F2}s");
    }

    void AddSentenceAsNewLine(VoiceOverCutsceneData data)
    {
        if (string.IsNullOrWhiteSpace(newSentenceText))
        {
            EditorUtility.DisplayDialog("Error", "Please enter some text first!", "OK");
            return;
        }

        // Calculate starting time based on all previous content
        float startTime = 0f;
        if (data.lines.Count > 0)
        {
            CutsceneLine lastLine = data.lines[data.lines.Count - 1];
            if (lastLine.sentences.Count > 0)
            {
                SentenceTimestamp lastSentence = lastLine.sentences[lastLine.sentences.Count - 1];
                startTime = lastSentence.startTime + lastLine.fadeOutDuration + lastLine.pauseAfterLine;
            }
        }

        // Create new line with sentence
        CutsceneLine newLine = new CutsceneLine();
        newLine.fullText = newSentenceText;
        newLine.sentences = new List<SentenceTimestamp>();
        newLine.fadeOutDuration = 0.5f;
        newLine.pauseAfterLine = 0.5f;

        SentenceTimestamp sentence = new SentenceTimestamp
        {
            sentence = newSentenceText,
            startTime = startTime,
            endTime = 0f, // Optional
            fadeInDuration = fadeInDuration
        };

        newLine.sentences.Add(sentence);
        data.lines.Add(newLine);

        EditorUtility.SetDirty(data);
        newSentenceText = "";

        Debug.Log($"Added new line starting at {startTime:F2}s");
    }
}
#endif