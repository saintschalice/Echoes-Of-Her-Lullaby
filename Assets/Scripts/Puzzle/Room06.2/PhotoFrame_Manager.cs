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
    private bool isSequencePlaying = false; // Panangga para di mag-doble click

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        sr.sprite = normalPhoto;

        if (photoUIPanel != null) photoUIPanel.SetActive(false);
    }

    public void Interact()
    {
        // Wag pansinin ang click kung may tumatakbo nang sequence o dialogue
        if (isSequencePlaying) return;
        if (DialogueSystemV2.Instance != null && DialogueSystemV2.Instance.IsDialogueActive()) return;

        // Distance Check
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Vector2.Distance(transform.position, player.transform.position) > interactionRange) return;

        // Simulan ang Sequence
        StartCoroutine(InteractionSequence());
    }

    private IEnumerator InteractionSequence()
    {
        isSequencePlaying = true;

        // 1. OPEN UI POP-UP
        if (photoUIPanel != null)
        {
            photoUIPanel.SetActive(true);
            photoUIImage.sprite = isTransformed ? distortedPhoto : normalPhoto;
        }

        // 2. TRIGGER NORMAL DIALOGUE
        if (!isTransformed)
        {
            // ITO YUNG KINOPYA NATIN MULA SA DINING ROOM SCRIPT MO!
            if (DialogueSystemV2.Instance != null)
                DialogueSystemV2.Instance.StartDialogue(dialogueBeforeScare, "Lisa");

            // Hihintayin matapos ang dialogue ni Lisa bago magulat
            yield return new WaitForSeconds(0.1f); // maliit na delay para di mag-skip
            yield return new WaitUntil(() => DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());

            // 3. THE SCARE & TRANSFORMATION!
            if (audioSource != null && transformationSound != null)
                audioSource.PlayOneShot(transformationSound);

            if (photoUIImage != null) photoUIImage.sprite = distortedPhoto;
            sr.sprite = distortedPhoto;
            transform.rotation = Quaternion.Euler(0, 0, -8f);
            isTransformed = true;

            // PLAY SCARE DIALOGUE
            if (DialogueSystemV2.Instance != null)
                DialogueSystemV2.Instance.StartDialogue(dialogueDuringScare, "Lisa");

            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());
        }
        else
        {
            // KUNG NA-TRANSFORM NA (Pag pinindot ulit ni player mamaya)
            if (DialogueSystemV2.Instance != null)
                DialogueSystemV2.Instance.StartDialogue(dialogueAfterScare, "Lisa");

            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(() => DialogueSystemV2.Instance == null || !DialogueSystemV2.Instance.IsDialogueActive());
        }

        // 4. CLOSE UI POP-UP PAGKATAPOS NG LAHAT
        if (photoUIPanel != null)
        {
            photoUIPanel.SetActive(false);
        }

        isSequencePlaying = false;
    }

    // --- IINTERACTABLE ---
    public void OnInteract(PlayerContext context) => Interact();
    public void OnFocus(PlayerContext context) { }
    public void OnBlur(PlayerContext context) { }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}