using UnityEngine;

public class DialogueTesterV2 : MonoBehaviour
{
    void Update()
    {
        // Press T to test dialogue
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestSingleLine();
        }

        // Press Y to test multi-line dialogue
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TestMultiLineDialogue();
        }

        // Press U to test all speakers
        if (Input.GetKeyDown(KeyCode.U))
        {
            TestAllSpeakers();
        }
    }

    void TestSingleLine()
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

    void TestMultiLineDialogue()
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

    void TestAllSpeakers()
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