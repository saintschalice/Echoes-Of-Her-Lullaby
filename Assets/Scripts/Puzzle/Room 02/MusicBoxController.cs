using UnityEngine;
using System.Collections;

public class MusicBoxController : MonoBehaviour, IInteractable
{
    [Header("IDs")]
    [SerializeField] private string brokenMusicBoxId = "broken_music_box";
    [SerializeField] private string windingKeyId = "winding_key";
    [SerializeField] private string completeMusicBoxId = "music_box_complete";

    [Header("Cutscene Data")]
    [Tooltip("Assign the Music Box Reveal VoiceOverCutsceneData asset here.")]
    public VoiceOverCutsceneData musicBoxCutscene;

    [Header("State")]
    [SerializeField] private bool isComplete;
    [SerializeField] private bool cutscenePlayed;

    public void TryCombine()
    {
        if (isComplete) return;

        bool hasBox = SaveSystem.Instance != null && SaveSystem.Instance.HasItem(brokenMusicBoxId);
        bool hasKey = SaveSystem.Instance != null && SaveSystem.Instance.HasItem(windingKeyId);

        if (hasBox && hasKey)
        {
            InventoryManager.Instance?.RemoveItem(brokenMusicBoxId);
            InventoryManager.Instance?.RemoveItem(windingKeyId);
            InventoryManager.Instance?.AddItem(completeMusicBoxId);

            isComplete = true;
            PlayRevealCutscene();
        }
    }

    public void PlayRevealCutscene()
    {
        if (cutscenePlayed) return;
        StartCoroutine(BeginCutsceneSequence());
    }

    // =================================================================================
    // FIX: Added parameterless Interact() method for PlayerInteractionTracker (Button)
    // =================================================================================
    public void Interact()
    {
        OnExamine();
    }
    // =================================================================================

    public void OnInteract(PlayerContext context)
    {
        OnExamine();
    }

    public void OnFocus(PlayerContext context) { }

    public void OnBlur(PlayerContext context) { }

    public void OnExamine()
    {
        if (!isComplete && SaveSystem.Instance != null && SaveSystem.Instance.HasItem(completeMusicBoxId))
            isComplete = true;

        if (!isComplete)
        {
            DialogueSystemV2.Instance?.StartDialogue("A delicate music box. Something seems to be missing.", "Lisa");
            return;
        }

        if (!cutscenePlayed)
        {
            StartCoroutine(BeginCutsceneSequence());
            return;
        }

        DialogueSystemV2.Instance?.StartDialogue("It's fixed. The melody reminds me of something...", "Lisa");
    }

    private IEnumerator BeginCutsceneSequence()
    {
        cutscenePlayed = true;
        InventoryManager.Instance?.CloseInventoryUI();

        if (musicBoxCutscene != null && CutsceneManager.Instance != null)
        {
            bool finished = false;
            CutsceneManager.Instance.PlayCutscene(musicBoxCutscene, () => finished = true);
            while (!finished)
                yield return null;
        }
        else
        {
            Debug.LogWarning("[MusicBoxController] No cutscene data assigned!");
            yield return new WaitForSeconds(2f);
        }

        Room02_LivingRoomController roomController = FindFirstObjectByType<Room02_LivingRoomController>();
        if (roomController != null)
            roomController.OnMusicBoxCutsceneEnded();
        else
            Debug.LogError("[MusicBoxController] Could not find Room02_LivingRoomController.");
    }
}