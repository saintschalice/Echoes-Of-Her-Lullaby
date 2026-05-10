using UnityEngine;
using UnityEngine.UI;

public class CurtainPuzzle : MonoBehaviour
{
    [Header("Curtain Sprites (I-drag mula sa hiniwang image)")]
    public Sprite bothClosed;
    public Sprite leftOpen;
    public Sprite rightOpen;
    public Sprite bothOpen;

    [Header("UI Reference")]
    public Image windowDisplay; // Yung Image component kung saan papalitan yung sprite

    // Internal logic
    private bool isLeftOpen = false;
    private bool isRightOpen = false;

    private void Start()
    {
        // I-set sa default na nakasara lahat
        UpdateWindowVisuals();
    }

    // I-connect ito sa OnClick() ng Left Invisible Button
    public void ToggleLeftCurtain()
    {
        isLeftOpen = !isLeftOpen; // Kung nakasara, bubuksan. Kung bukas, isasara.
        UpdateWindowVisuals();
    }

    // I-connect ito sa OnClick() ng Right Invisible Button
    public void ToggleRightCurtain()
    {
        isRightOpen = !isRightOpen;
        UpdateWindowVisuals();
    }

    private void UpdateWindowVisuals()
    {
        // Papalitan yung picture depende sa kung ano ang binuksan ni player
        if (!isLeftOpen && !isRightOpen)
            windowDisplay.sprite = bothClosed;
        else if (isLeftOpen && !isRightOpen)
            windowDisplay.sprite = leftOpen;
        else if (!isLeftOpen && isRightOpen)
            windowDisplay.sprite = rightOpen;
        else if (isLeftOpen && isRightOpen)
        {
            windowDisplay.sprite = bothOpen;
            PuzzleSolved();
        }
    }

    private void PuzzleSolved()
    {
        Debug.Log("Curtains fully opened!");
        // Tatawagin natin dito yung UIManager para masave ang progress
        Room07UIManager uiManager = GetComponentInParent<Room07UIManager>();
        if (uiManager != null)
        {
            uiManager.OnCurtainsOpened(); // Sasabihin sa manager na tapos na
        }
    }
}
