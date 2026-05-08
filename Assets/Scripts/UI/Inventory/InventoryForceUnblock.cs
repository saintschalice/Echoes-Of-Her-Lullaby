using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Emergency script to force unblock inventory when it gets stuck.
/// Attach to InventoryPanel or any GameObject.
/// Press F key to force unblock.
/// </summary>
public class InventoryForceUnblock : MonoBehaviour
{
    [Header("Debug Key")]
    public KeyCode unblockKey = KeyCode.F;

    void Update()
    {
        if (Input.GetKeyDown(unblockKey))
        {
            ForceUnblockInventory();
        }
    }

    [ContextMenu("Force Unblock Inventory")]
    public void ForceUnblockInventory()
    {
        Debug.Log("=== FORCE UNBLOCKING INVENTORY ===");

        // 1. Find InventoryPanel
        GameObject inventoryPanel = GameObject.Find("InventoryPanel");
        if (inventoryPanel == null)
        {
            inventoryPanel = GameObject.Find("Inventory Panel");
        }
        if (inventoryPanel == null)
        {
            Debug.LogError("[ForceUnblock] InventoryPanel not found!");
            return;
        }

        Debug.Log($"[ForceUnblock] Found InventoryPanel: {inventoryPanel.name}");

        // 2. Fix CanvasGroup
        CanvasGroup cg = inventoryPanel.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            Debug.Log($"[ForceUnblock] Before - Alpha: {cg.alpha}, Interactable: {cg.interactable}, BlocksRaycasts: {cg.blocksRaycasts}");
            
            cg.alpha = 1f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            
            Debug.Log($"[ForceUnblock] After - Alpha: {cg.alpha}, Interactable: {cg.interactable}, BlocksRaycasts: {cg.blocksRaycasts}");
        }
        else
        {
            Debug.LogWarning("[ForceUnblock] No CanvasGroup on InventoryPanel!");
        }

        // 3. Close any blocking UI
        CloseBlockingUI();

        // 4. Re-enable EventSystem
        UnityEngine.EventSystems.EventSystem eventSystem = FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>();
        if (eventSystem != null)
        {
            eventSystem.enabled = true;
            Debug.Log("[ForceUnblock] EventSystem enabled");
        }

        // 5. Refresh InventoryUI
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RefreshInventory();
            Debug.Log("[ForceUnblock] InventoryUI refreshed");
        }

        Debug.Log("=== INVENTORY UNBLOCKED ===");
        Debug.Log("Try clicking items now!");
    }

    void CloseBlockingUI()
    {
        // Close Recipe Book if open
        if (RecipeBookUI.Instance != null)
        {
            RecipeBookUI.Instance.CloseBook();
            Debug.Log("[ForceUnblock] Closed RecipeBookUI");
        }

        // Close Diary if open
        DiaryReaderUI diaryReader = FindFirstObjectByType<DiaryReaderUI>();
        if (diaryReader != null)
        {
            diaryReader.CloseDiary();
            Debug.Log("[ForceUnblock] Closed DiaryReaderUI");
        }

        // Close Mail if open
        MailReaderUI mailReader = FindFirstObjectByType<MailReaderUI>();
        if (mailReader != null)
        {
            mailReader.CloseMail();
            Debug.Log("[ForceUnblock] Closed MailReaderUI");
        }

        // Close any other panels that might be blocking
        GameObject[] allPanels = GameObject.FindGameObjectsWithTag("UIPanel");
        foreach (GameObject panel in allPanels)
        {
            if (panel.activeSelf && panel.name.Contains("Recipe") || panel.name.Contains("Book"))
            {
                panel.SetActive(false);
                Debug.Log($"[ForceUnblock] Closed panel: {panel.name}");
            }
        }
    }

    [ContextMenu("Debug Inventory State")]
    public void DebugInventoryState()
    {
        Debug.Log("=== INVENTORY STATE DEBUG ===");

        // Check InventoryUI
        if (InventoryUI.Instance != null)
        {
            Debug.Log($"[Debug] InventoryUI exists, IsOpen: {InventoryUI.Instance.IsOpen}");
        }
        else
        {
            Debug.LogError("[Debug] InventoryUI.Instance is NULL!");
        }

        // Check InventoryPanel
        GameObject inventoryPanel = GameObject.Find("InventoryPanel");
        if (inventoryPanel != null)
        {
            Debug.Log($"[Debug] InventoryPanel active: {inventoryPanel.activeSelf}");

            CanvasGroup cg = inventoryPanel.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                Debug.Log($"[Debug] CanvasGroup - Alpha: {cg.alpha}, Interactable: {cg.interactable}, BlocksRaycasts: {cg.blocksRaycasts}");
            }
        }
        else
        {
            Debug.LogError("[Debug] InventoryPanel not found!");
        }

        // Check for blocking UI
        if (RecipeBookUI.Instance != null)
        {
            Debug.Log($"[Debug] RecipeBookUI panel active: {RecipeBookUI.Instance.panel?.activeSelf}");
        }

        Debug.Log("=== END DEBUG ===");
    }
}
