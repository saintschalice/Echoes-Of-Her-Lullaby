using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PhotoFrame_Manager : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    public float interactionRange = 2f;

    [Header("UI Pop-up Settings")]
    public GameObject photoUIPanel;
    public Image photoUIImage;

    [Header("Sprites")]
    public Sprite normalPhoto;
    public Sprite distortedPhoto;

    [Header("Dialogue Content")]
    [TextArea] public string dialogueBeforeScare = "...This Place. I remember this.";
    [TextArea] public string dialogueDuringScare = "No... that's not right.";
    [TextArea] public string dialogueAfterScare = "It's getting worse...";

    [Header("Audio")]
    public AudioClip transformationSound;

    private SpriteRenderer sr;
    private AudioSource audioSource;
    private bool isTransformed = false;
    private bool isSequencePlaying = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        sr.sprite = normalPhoto;
        if (photoUIPanel != null) photoUIPanel.SetActive(false);
    }

    public void Interact()
    {
        if (isSequencePlaying) return;
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Vector2.Distance(transform.position, player.transform.position) > interactionRange) return;

        StartCoroutine(InteractionSequence());
    }

    private IEnumerator InteractionSequence()
    {
        isSequencePlaying = true;

        // SIGURADONG ISE-SAVE PARA BUMUKAS ANG PINTO!
        Debug.Log("[PhotoFrame] Sini-save ang progress sa R06_PhotoInteracted = 1");
        PlayerPrefs.SetInt("R06_PhotoInteracted", 1);
        PlayerPrefs.Save();

        // PAUSE GAME FOR UI
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        JoystickPlayerController playerController = null;
        EmilyGhost emilyAI = null;

        if (playerObj != null)
        {
            playerController = playerObj.GetComponent<JoystickPlayerController>();
            if (playerController != null) playerController.enabled = false;
        }

        emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = true;

        if (photoUIPanel != null) {
            photoUIPanel.SetActive(true);
            photoUIImage.sprite = isTransformed ? distortedPhoto : normalPhoto;
        }

        if (!isTransformed)
        {
            if (DialogueSystemV2.Instance != null) 
                DialogueSystemV2.Instance.StartDialogue(dialogueBeforeScare, "Lisa");
            
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());

            if (audioSource != null && transformationSound != null)
                audioSource.PlayOneShot(transformationSound);

            if (photoUIImage != null) photoUIImage.sprite = distortedPhoto;
            sr.sprite = distortedPhoto;
            transform.rotation = Quaternion.Euler(0, 0, -8f); 
            isTransformed = true;

            if (DialogueSystemV2.Instance != null) 
                DialogueSystemV2.Instance.StartDialogue(dialogueDuringScare, "Lisa");

            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());
            
            // --- PALABASIN SI EMILY! ---
            Debug.Log("[PhotoFrame] Tinatawag na ang Hallway Controller para kay Emily...");
            if (Room06_HallwayController.Instance != null)
            {
                Room06_HallwayController.Instance.TriggerEmilyChase();
            }
            else
            {
                Debug.LogError("[PhotoFrame] ERROR: Hindi mahanap ang Room06_HallwayController.Instance!");
            }
        }
        else
        {
            if (DialogueSystemV2.Instance != null) 
                DialogueSystemV2.Instance.StartDialogue(dialogueAfterScare, "Lisa");

            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());
        }

        if (photoUIPanel != null) photoUIPanel.SetActive(false);
        
        // RESUME GAME FROM UI
        if (playerController != null) playerController.enabled = true;
        if (emilyAI != null) emilyAI.isPaused = false;
        
        isSequencePlaying = false;
    }

    public void OnInteract(PlayerContext context) => Interact();
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}