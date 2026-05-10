using UnityEngine;
using TMPro;

public class NoteOverlayManager : MonoBehaviour
{
    public static NoteOverlayManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject noteUIRoot; // I-drag ang GenericNoteUI dito
    public TextMeshProUGUI titleText; // I-drag ang Title Text dito
    public TextMeshProUGUI contentText; // I-drag ang Content Text dito

    // Dito natin ise-save kung ano yung function na tatawagin pagka-close
    private System.Action onNoteClosedCallback;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Ito ang tatawagin mo mula sa ibang scripts para magpakita ng Note
    public void ShowNote(string title, string content, System.Action onClose)
    {
        if (titleText != null) titleText.text = title;
        if (contentText != null) contentText.text = content;

        onNoteClosedCallback = onClose; // I-save kung anong gagawin pag tapos na

        noteUIRoot.SetActive(true);
    }

    // Ito ang tatawagin ng "Sige, patuloy" Button
    public void OnClickContinue()
    {
        noteUIRoot.SetActive(false); // Itago ang UI

        // Kung may pinapasang action nung binuksan, i-run yun ngayon (hal. ResumeGame)
        if (onNoteClosedCallback != null)
        {
            onNoteClosedCallback.Invoke();
            onNoteClosedCallback = null; // I-reset
        }
    }
}