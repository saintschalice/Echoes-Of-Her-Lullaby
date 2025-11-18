using UnityEngine;
using System.Collections;

public class MusicBoxController : MonoBehaviour
{
    [Header("IDs")]
    [SerializeField] private string brokenMusicBoxId = "broken_music_box";
    [SerializeField] private string windingKeyId = "winding_key";
    [SerializeField] private string completeMusicBoxId = "music_box_complete";
    // Removed: [SerializeField] private string hallwayKeyId = "hallway_door_key"; // Logic moved to RoomController

    [Header("Cutscene Data")]
    [Tooltip("Assign the Music Box Reveal VoiceOverCutsceneData asset here.")]
    public VoiceOverCutsceneData musicBoxCutscene;

    [Header("State")]
    [SerializeField] private bool isComplete;
    [SerializeField] private bool cutscenePlayed;

    // Called when player selects “Combine” in inventory
    public void TryCombine()
    {
        // NOTE: This legacy method might be called by older systems.
        // ItemExaminationHandler now handles combination and calls PlayRevealCutscene directly.
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
            PlayRevealCutscene();
        }
    }

    // Public entry point for the cutscene
    public void PlayRevealCutscene()
    {
        if (cutscenePlayed) return;
        StartCoroutine(BeginCutsceneSequence());
    }

    // Called when the player interacts with the music box object in the world
    public void OnExamine()
    {
        // Update complete status based on inventory just in case
        if (!isComplete && SaveSystem.Instance != null && SaveSystem.Instance.HasItem(completeMusicBoxId))
        {
            isComplete = true;
        }

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

        DialogueSystemV2.Instance?.StartDialogue(
             "It's fixed. The melody reminds me of something...",
             "Lisa"
         );
    }


    // Sequence that plays the cutscene THEN triggers room logic
    private IEnumerator BeginCutsceneSequence()
    {
        cutscenePlayed = true;

        // Close any open UI (Inventory etc)
        InventoryManager.Instance?.CloseInventoryUI();

        // 1. Play the cutscene
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
            Debug.LogWarning("[MusicBoxController] No cutscene data assigned!");
            yield return new WaitForSeconds(2f);
        }

        // 2. Notify RoomController to arm the Hallway Event Trigger
        // We do NOT give the key here anymore.
        Room02_LivingRoomController roomController = FindFirstObjectByType<Room02_LivingRoomController>();
        if (roomController != null)
        {
            roomController.OnMusicBoxCutsceneEnded();
        }
        else
        {
            Debug.LogError("[MusicBoxController] Could not find Room02_LivingRoomController to trigger post-cutscene events.");
        }
    }
}