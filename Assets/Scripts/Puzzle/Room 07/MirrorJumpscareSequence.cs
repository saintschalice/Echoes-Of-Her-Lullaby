using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the mirror jumpscare and initiates the chase sequence
/// </summary>
public class MirrorJumpscareSequence : MonoBehaviour
{
    [Header("Jumpscare Elements")]
    public GameObject emilyGhostObject; // Emily AI GameObject
    public Transform emilyJumpscarePosition; // Position behind Lisa
    public GameObject jumpscareImage; // Full-screen Emily image
    public float jumpscareDuration = 2f;

    [Header("Audio")]
    public AudioClip jumpscareSound;
    public AudioClip lullabyFragment3;
    public AudioSource musicBoxSource; // Source from toybox

    [Header("Door Locking")]
    public GameObject bedroomDoorCollider; // Main door to lock
    public GameObject bathroomDoor; // Escape route

    [Header("Camera Shake")]
    public float shakeIntensity = 0.3f;
    public float shakeDuration = 0.5f;

    private bool hasTriggered = false;

    public void TriggerJumpscare()
    {
        if (hasTriggered) return;

        // Check if all puzzles are complete
        Room07_FlowController flow = Room07_FlowController.Instance;
        if (flow == null) return;

        if (!flow.areCurtainsOpened || !flow.isTeaPartyDone || 
            !flow.isToyboxSolved || !flow.isDollhouseDone)
        {
            DialogueSystemV2.Instance?.StartDialogue("I feel like I'm still missing something in here...", "Lisa");
            return;
        }

        hasTriggered = true;
        StartCoroutine(JumpscareSequence());
    }

    IEnumerator JumpscareSequence()
    {
        // Disable player controls
        JoystickPlayerController player = FindFirstObjectByType<JoystickPlayerController>();
        if (player != null) player.enabled = false;

        GameObject joystick = GameObject.Find("Joystick");
        if (joystick != null) joystick.SetActive(false);

        // NOTE: Dialogues are handled by Room07_Interactable.cs before calling this
        // No need to show dialogues here to avoid duplicates

        yield return new WaitForSeconds(0.5f);

        // 1. JUMPSCARE!
        if (jumpscareSound != null)
            AudioManager.Instance?.PlaySFX(jumpscareSound);

        // Show jumpscare image
        if (jumpscareImage != null)
        {
            jumpscareImage.SetActive(true);
        }

        // Spawn Emily behind Lisa
        if (emilyGhostObject != null && emilyJumpscarePosition != null)
        {
            emilyGhostObject.transform.position = emilyJumpscarePosition.position;
            emilyGhostObject.SetActive(true);
        }

        // Camera shake
        StartCoroutine(ShakeCamera());

        yield return new WaitForSeconds(jumpscareDuration);

        // Hide jumpscare image
        if (jumpscareImage != null)
        {
            jumpscareImage.SetActive(false);
        }

        // 2. Play Lullaby Fragment #3
        if (lullabyFragment3 != null && musicBoxSource != null)
        {
            musicBoxSource.clip = lullabyFragment3;
            musicBoxSource.Play();
            
            // Wait for lullaby to finish
            yield return new WaitForSeconds(lullabyFragment3.length);
        }
        else
        {
            yield return new WaitForSeconds(3f); // Fallback duration
        }

        yield return new WaitForSeconds(0.5f);

        // 3. Lock the bedroom door
        if (bedroomDoorCollider != null)
        {
            // Change tag or disable collider to prevent exit
            bedroomDoorCollider.tag = "Untagged";
            // Or: bedroomDoorCollider.SetActive(false);
        }

        // Re-enable player controls for chase
        if (player != null) player.enabled = true;
        if (joystick != null) joystick.SetActive(true);

        // 4. Activate Emily's aggressive chase AI
        if (emilyGhostObject != null)
        {
            EmilyGhost emily = emilyGhostObject.GetComponent<EmilyGhost>();
            if (emily != null)
            {
                emily.isPaused = false;
                
                // Make Emily chase faster and more aggressively
                emily.huntSpeed = 3.5f; // Increase hunt speed for chase
                emily.lostLOSTime = 5f; // Takes longer to lose sight of player
                
                // Force Emily into Hunt state
                emily.SetStateExternal(EmilyGhost.State.Hunt);
            }
        }

        Debug.Log("[Room07] Chase sequence started! Run to the bathroom!");
    }

    IEnumerator ShakeCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) yield break;

        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }
}
