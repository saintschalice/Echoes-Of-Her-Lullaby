using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MailReaderUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mailPanel;
    public Image mailImage; // The mail sprite/image
    public TextMeshProUGUI mailContentText;
    public Button closeButton;

    [Header("Mail Content")]
    [TextArea(5, 10)]
    public string mailContent = @"To the current resident,

This house holds secrets that have been buried for decades. 
If you're reading this, you're meant to find the truth.

Look beyond what you see. 
The flowers hide more than beauty.
Break the surface to reveal what lies beneath.

- A Friend";

    [Header("Audio")]
    public AudioClip openMailSound;
    public AudioClip closeMailSound;

    private AudioSource audioSource;
    private bool hasBeenRead = false;

    // Save state identifier
    private const string MAIL_READ_ID = "Foyer_Mail_Read";

    public static MailReaderUI Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Setup audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        // Setup UI
        if (mailPanel != null)
        {
            mailPanel.SetActive(false);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseMail);
        }

        // Check save state
        CheckSaveState();
    }

    public void OpenMail()
    {
        if (mailPanel == null) return;

        // Mark as read
        hasBeenRead = true;

        // Save read state
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.MarkObjectExamined(MAIL_READ_ID);
        }

        // Play sound
        if (openMailSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(openMailSound);
        }

        // Show mail panel
        mailPanel.SetActive(true);

        // Set content
        if (mailContentText != null)
        {
            mailContentText.text = mailContent;
        }

        // Pause game
        Time.timeScale = 0f;

        Debug.Log("Mail opened and read!");
    }

    public void CloseMail()
    {
        if (mailPanel == null) return;

        // Play sound
        if (closeMailSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(closeMailSound);
        }

        // Hide panel
        mailPanel.SetActive(false);

        // Resume game
        Time.timeScale = 1f;

        // No hint dialogue - player discovers on their own
    }

    void CheckSaveState()
    {
        if (SaveSystem.Instance != null)
        {
            hasBeenRead = SaveSystem.Instance.WasObjectExamined(MAIL_READ_ID);
        }
    }

    public bool HasBeenRead()
    {
        return hasBeenRead;
    }

    void Update()
    {
        // Allow closing with ESC key
        if (mailPanel != null && mailPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMail();
        }
    }
}