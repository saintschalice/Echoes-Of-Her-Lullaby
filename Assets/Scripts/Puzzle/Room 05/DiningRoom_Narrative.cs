using UnityEngine;
using TMPro;

public class DiningRoom_Narrative : MonoBehaviour
{
    private TextMeshProUGUI uiText;

    void Start()
    {
        // Hahanapin nito ang TextMeshPro sa Persistent Scene. 
        // Siguraduhin na "DialogueText" ang pangalan ng UI object mo.
        GameObject textObj = GameObject.Find("DialogueText");
        if (textObj != null) uiText = textObj.GetComponent<TextMeshProUGUI>();
    }

    public void ShowMessage(string message)
    {
        if (uiText != null) uiText.text = message;
    }
}