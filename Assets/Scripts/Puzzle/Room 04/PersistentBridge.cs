using UnityEngine;

public class PersistentBridge : MonoBehaviour
{
    [Header("References")]
    public GameObject bridgeVisual;  // Yung Floorboard sprite na lilitaw
    public GameObject gapBlocker;    // Yung harang na collider

    [Header("Settings")]
    public string uniqueID = "Room04_Bridge_Fixed"; // Ito ang "Memory Key"

    void Start()
    {
        // 1. CHECK SA START: Nagawa na ba to dati? (1 = True, 0 = False)
        if (PlayerPrefs.GetInt(uniqueID, 0) == 1)
        {
            // Kung nagawa na, i-activate agad ang tulay
            ShowBridge();
        }
    }

    // Tawagin function na 'to kapag nilagay ni Lisa ang floorboard
    public void SaveBridgeState()
    {
        // I-save sa memory na "Okay na ang tulay na 'to"
        PlayerPrefs.SetInt(uniqueID, 1);
        PlayerPrefs.Save();

        Debug.Log("Bridge progress SAVED!");
        ShowBridge();
    }

    void ShowBridge()
    {
        if (bridgeVisual != null) bridgeVisual.SetActive(true);
        if (gapBlocker != null) gapBlocker.SetActive(false);
    }

    // Debug Tool: Pindutin ang F5 habang naglalaro para i-reset kung nagkamali ka
    void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.F5))
        {
            PlayerPrefs.DeleteKey(uniqueID);
            Debug.Log("Bridge Reset! (Restart Scene to see effect)");
        }
    }
}