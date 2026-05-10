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

    [Header("Audio - SFX")]
    public AudioClip openMailSound;
    public AudioClip closeMailSound;

    // REMOVED: No more AudioSource needed!
    private bool hasBeenRead = false;

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

        Debug.Log("[MailReader] MailReaderUI initialized with AudioManager integration");
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

        // NEW: Play sound through AudioManager (categorized as SFX)
        if (openMailSound != null)
        {
            AudioManager.Instance?.PlaySFX(openMailSound);
        }

        // Show mail panel
        mailPanel.SetActive(true);

        // Set content
        if (mailContentText != null)
        {
            mailContentText.text = mailContent;
        }

        // NEW: Force close inventory when mail opens
        InventoryUI inventoryUI = FindFirstObjectByType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.ForceCloseInventory();
        }

        // Pause game
        Time.timeScale = 0f;

        // Pause Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = true;

        Debug.Log("[MailReader] Mail opened and read!");
    }

    public void CloseMail()
    {
        if (mailPanel == null) return;

        // NEW: Play sound through AudioManager (categorized as SFX)
        if (closeMailSound != null)
        {
            AudioManager.Instance?.PlaySFX(closeMailSound);
        }

        // Hide panel
        mailPanel.SetActive(false);

        // Resume game
        Time.timeScale = 1f;

        // Resume Emily AI
        EmilyGhost emilyAI = FindFirstObjectByType<EmilyGhost>();
        if (emilyAI != null) emilyAI.isPaused = false;

        Debug.Log("[MailReader] Mail closed");

        InventoryManager.Instance?.NotifyActionEnded();
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

    // Optional: Public method to check if mail panel is currently open
    public bool IsMailOpen()
    {
        return mailPanel != null && mailPanel.activeSelf;
    }
}