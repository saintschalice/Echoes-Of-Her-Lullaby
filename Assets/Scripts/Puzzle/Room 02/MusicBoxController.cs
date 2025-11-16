using UnityEngine;
using System.Collections;

public class MusicBoxController : MonoBehaviour
{
    [Header("IDs")]
    [SerializeField] private string brokenMusicBoxId = "broken_music_box";
    [SerializeField] private string windingKeyId = "winding_key";
    [SerializeField] private string completeMusicBoxId = "music_box_complete";
    [SerializeField] private string hallwayKeyId = "hallway_door_key";

    [Header("Cutscene Data")]
    [Tooltip("Assign the Music Box Reveal VoiceOverCutsceneData asset here.")]
    public VoiceOverCutsceneData musicBoxCutscene;

    [Header("State")]
    [SerializeField] private bool isComplete;
    [SerializeField] private bool cutscenePlayed;
    [SerializeField] private bool hallwayKeyGiven;


    // Called when player selects “Combine” in inventory
    public void TryCombine()
    {
        if (isComplete) return;

        bool hasBox = SaveSystem.Instance != null && SaveSystem.Instance.HasItem(brokenMusicBoxId);
        bool hasKey = SaveSystem.Instance != null && SaveSystem.Instance.HasItem(windingKeyId);

        if (hasBox && hasKey)
        {
            // Remove broken parts, add complete music box
            InventoryManager.Instance?.RemoveItem(brokenMusicBoxId);
            InventoryManager.Instance?.RemoveItem(windingKeyId);
            InventoryManager.Instance?.AddItem(completeMusicBoxId);

            isComplete = true;

            DialogueSystemV2.Instance?.StartDialogue(
                "It fits perfectly...",
                "Lisa"
            );
        }
    }


    // Called when the player interacts with the music box object in the world
    public void OnExamine()
    {
        if (!isComplete)
        {
            DialogueSystemV2.Instance?.StartDialogue(
                "A delicate music box. Something seems to be missing.",
                "Lisa"
            );
            return;
        }

        // Cutscene hasn't been played → start it
        if (!cutscenePlayed)
        {
            StartCoroutine(BeginCutsceneSequence());
            return;
        }

        // Already complete + key given
        if (hallwayKeyGiven)
        {
            DialogueSystemV2.Instance?.StartDialogue(
                "I already got the key. Time to move on.",
                "Lisa"
            );
        }
    }


    // Sequence that plays the cutscene THEN gives the hallway key
    private IEnumerator BeginCutsceneSequence()
    {
        cutscenePlayed = true;

        // If cutscene exists: call it properly (CutsceneManager does NOT return IEnumerator)
        if (musicBoxCutscene != null && CutsceneManager.Instance != null)
        {
            bool finished = false;

            CutsceneManager.Instance.PlayCutscene(musicBoxCutscene, () =>
            {
                finished = true;
            });

            // Wait until CutsceneManager says it's done
            while (!finished)
                yield return null;
        }
        else
        {
            // No cutscene available → simple delay placeholder
            yield return new WaitForSeconds(2f);
        }

        // Grant the hallway key AFTER the cutscene
        if (!hallwayKeyGiven)
        {
            hallwayKeyGiven = true;
            InventoryManager.Instance?.AddItem(hallwayKeyId);

            DialogueSystemV2.Instance?.StartDialogue(
                "I got it. Now to the next room.",
                "Lisa"
            );
        }
    }
}
