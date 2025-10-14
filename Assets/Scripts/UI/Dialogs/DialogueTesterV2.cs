using UnityEngine;
using System.Collections; // ADD THIS LINE - Required for IEnumerator

public class DialogueTesterV2 : MonoBehaviour
{
    void Update()
    {
        // Your existing Update code...

        // Add this new test
        if (Input.GetKeyDown(KeyCode.C))
        {
            TestChoiceSystem();
        }
    }

    void TestChoiceSystem()
    {
        DialogueSystemV2.Instance.StartDialogue("Should I break this pot?", "Lisa");
        StartCoroutine(ShowChoicesAfterDelay());
    }

    IEnumerator ShowChoicesAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        DialogueSystemV2.Instance.ShowChoices(
            new string[] { "Yes, break it", "No, leave it" },
            new System.Action[] { OnChoiceYes, OnChoiceNo }
        );
    }

    void OnChoiceYes()
    {
        Debug.Log("Player chose YES!");
        DialogueSystemV2.Instance.StartDialogue("You chose to break the pot!", "Lisa");
    }

    void OnChoiceNo()
    {
        Debug.Log("Player chose NO!");
        DialogueSystemV2.Instance.StartDialogue("You decided to leave it alone.", "Lisa");
    }

    public void TestSingleLine()
    {
        if (DialogueSystemV2.Instance != null)
        {
            DialogueSystemV2.Instance.StartDialogue("This is a test message!", "Lisa");
        }
        else
        {
            Debug.LogError("DialogueSystemV2.Instance is null!");
        }
    }

    public void TestMultiLineDialogue()
    {
        if (DialogueSystemV2.Instance != null)
        {
            DialogueLine[] lines = new DialogueLine[]
            {
                new DialogueLine { text = "This room feels familiar...", speakerName = "Lisa" },
                new DialogueLine { text = "But I don't remember being here before.", speakerName = "Lisa" },
                new DialogueLine { text = "Maybe I should look around.", speakerName = "Lisa" }
            };

            DialogueSystemV2.Instance.StartDialogue(lines);
        }
    }

    public void TestAllSpeakers()
    {
        if (DialogueSystemV2.Instance != null)
        {
            DialogueLine[] lines = new DialogueLine[]
            {
                new DialogueLine { text = "Hello, this is Lisa speaking.", speakerName = "Lisa" },
                new DialogueLine { text = "Who... who's there?", speakerName = "???" },
                new DialogueLine { text = "My name is Emily. I've been waiting for you.", speakerName = "Emily" }
            };

            DialogueSystemV2.Instance.StartDialogue(lines);
        }
    }
}