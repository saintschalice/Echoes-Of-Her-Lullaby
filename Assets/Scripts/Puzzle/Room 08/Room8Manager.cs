// Developer: Jhon Jellar Z. Miranda
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Room8Manager : MonoBehaviour
{
    public static Room8Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [Header("Story Progress")]
    public bool isBathtubInspected = false;
    public bool isCabinetInspected = false;
    public bool isMirrorUnlocked = false;
    private bool isQTEActive = false;
    public bool hasHammer = false;

    [Header("Room 8 UI Elements")]
    public GameObject qteButtonObject; // Yung malaking Tap Button
    public TextMeshProUGUI qteProgressText; // Yung text na "SWINGS: 0/15"
    public TextMeshProUGUI timerText; // BAGO: Yung text na magpapakita ng 10 seconds

    [Header("QTE Settings")]
    public int tapsRequired = 15;
    private int currentTaps = 0;

    [Header("Timer Settings")]
    public float qteTimeLimit = 10f; // 10 seconds na timer
    private float currentTimer;

    public void TryShowDialogue(string text, string characterName = "Lisa")
    {
        if (DialogueSystemV2.Instance != null)
            DialogueSystemV2.Instance.StartDialogue(text, characterName);
    }

    public void InteractWith(string objName)
    {
        if (isQTEActive) return;

        switch (objName)
        {
            case "Bathtub":
                isBathtubInspected = true;
                TryShowDialogue("Someone was badly hurt in here... a child was bleeding.");
                CheckMirrorUnlock();
                break;

            case "Cabinet":
                if (!hasHammer)
                {
                    isCabinetInspected = true;
                    hasHammer = true;

                    if (InventoryManager.Instance != null)
                    {
                        InventoryManager.Instance.AddItem("hammer");
                    }

                    TryShowDialogue("Child's bandages everywhere... Wait, there's a heavy hammer here. I'll take it.");
                    CheckMirrorUnlock();
                }
                else
                {
                    TryShowDialogue("Just empty medicine bottles and bandages left.");
                }
                break;

            case "Mirror":
                if (isMirrorUnlocked)
                {
                    bool playerHasHammer = hasHammer || (InventoryManager.Instance != null && InventoryManager.Instance.HasItem("hammer"));

                    if (playerHasHammer)
                    {
                        TryShowDialogue("Wait... there's light behind this. I can break it with the hammer!");
                        Invoke("StartQTESequence", 2.5f);
                    }
                    else
                    {
                        TryShowDialogue("This mirror feels weird, but I can't break it with my bare hands.");
                    }
                }
                else
                {
                    TryShowDialogue("It's just a dirty mirror...");
                }
                break;
        }
    }

    private void CheckMirrorUnlock()
    {
        if (isBathtubInspected && isCabinetInspected && !isMirrorUnlocked)
        {
            isMirrorUnlocked = true;
        }
    }

    private void StartQTESequence()
    {
        isQTEActive = true;
        currentTimer = qteTimeLimit; // I-reset ang timer sa 10 seconds
        currentTaps = 0;

        qteButtonObject.SetActive(true);

        if (qteProgressText != null) qteProgressText.text = "SWINGS: 0/" + tapsRequired;

        TryShowDialogue("No! Don't break it! You can't go there!", "Emily");
    }

    // BAGO: Dito natin chine-check ang countdown timer araw-araw (every frame)
    private void Update()
    {
        if (isQTEActive)
        {
            currentTimer -= Time.deltaTime; // Bawasan ang oras

            // I-update ang UI ng timer para makita ng player (e.g., "TIME: 8.5s")
            if (timerText != null)
            {
                timerText.text = "TIME: " + currentTimer.ToString("F1") + "s";

                // Gawing kulay pula ang text pag 3 seconds na lang!
                if (currentTimer <= 3f) timerText.color = Color.red;
                else timerText.color = Color.white;
            }

            // Kapag naubos ang oras bago mabasag ang salamin
            if (currentTimer <= 0f)
            {
                FailQTE();
            }
        }
    }

    public void OnMirrorTapped()
    {
        // LOG 1: Tingnan kung nare-register ba ng Unity yung click mo
        Debug.Log(">> [QTE TEST] NA-CLICK ANG BUTTON! isQTEActive = " + isQTEActive);

        if (!isQTEActive)
        {
            Debug.LogWarning(">> [QTE TEST] Hindi tinanggap ang click kasi isQTEActive ay FALSE.");
            return;
        }

        currentTaps++;

        // LOG 2: Tingnan kung nadadagdagan yung bilang
        Debug.Log(">> [QTE TEST] CURRENT SWINGS: " + currentTaps + " / " + tapsRequired);

        if (qteProgressText != null)
        {
            qteProgressText.text = "SWINGS: " + currentTaps + "/" + tapsRequired;
            // LOG 3: Confirm kung na-update ang text
            Debug.Log(">> [QTE TEST] Text updated to: SWINGS: " + currentTaps + "/" + tapsRequired);
        }
        else
        {
            // LOG 4: Baka nakalimutan mong i-assign sa Inspector!
            Debug.LogError(">> [QTE TEST] ERROR: Walang naka-assign na qteProgressText sa Inspector!");
        }

        // Optional: Dito ka pwede mag-trigger ng visual (e.g., palitan ang sprite ng basag na salamin)
        // if (currentTaps == 5) mirrorSpriteRenderer.sprite = crackedMirrorSprite1;

        if (currentTaps >= tapsRequired)
        {
            Debug.Log(">> [QTE TEST] SUCCESS! Wasak ang salamin!");
            FinishQTE();
        }
    }

    private void FinishQTE()
    {
        isQTEActive = false;
        qteButtonObject.SetActive(false);
        Debug.Log("Mirror Shatters! Lumilipat na sa Room 9...");
        SceneManager.LoadScene("Room09_Master's_Bathroom");
    }

    // BAGO: Kapag naubos ang 10 seconds
    private void FailQTE()
    {
        isQTEActive = false;
        qteButtonObject.SetActive(false);
        Debug.Log("GAME OVER! Naabutan ni Emily si Lisa!");

        TryShowDialogue("I caught you...", "Emily");

        // ILAGAY DITO ANG LOGIC MO PARA SA GAME OVER
        // Example: SceneManager.LoadScene("GameOverScene");
        // O kaya tawagin mo yung jumpscare SFX at panel mo dito.
    }
}