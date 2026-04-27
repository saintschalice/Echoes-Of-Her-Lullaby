using UnityEngine;
using UnityEngine.UI;

public class CabinetPuzzleUI : MonoBehaviour
{
    [Header("UI References")]
    public Image[] digitDisplayImages; // I-drag dito yung 6 na Image objects ng digits
    public Sprite[] numberSprites;     // I-drag dito ang 10 sprites (0 to 9)

    [Header("Padlock")]
    public Image padlockImage;
    public Sprite unlockedSprite;

    private int[] currentDigits = new int[6];

    // Tatawagin ito ng bawat Digit Button (Index 0-5)
    public void OnDigitClicked(int index)
    {
        // Increment index 0-9
        currentDigits[index] = (currentDigits[index] + 1) % 10;

        // Palitan ang Sprite base sa bagong number
        UpdateDigitSprite(index);

        // Mag-play ng mechanical click SFX dito
    }

    void UpdateDigitSprite(int index)
    {
        int spriteIndex = currentDigits[index];
        digitDisplayImages[index].sprite = numberSprites[spriteIndex];
    }

    public string GetEnteredCode()
    {
        // Pinagsasama ang 6 numbers para maging isang string (e.g., "332412")
        return string.Join("", currentDigits);
    }

    public void ResetPuzzle()
    {
        for (int i = 0; i < currentDigits.Length; i++)
        {
            currentDigits[i] = 0;
            UpdateDigitSprite(i);
        }
    }

    public void ShowUnlockVisual()
    {
        if (unlockedSprite != null) padlockImage.sprite = unlockedSprite;
    }
}