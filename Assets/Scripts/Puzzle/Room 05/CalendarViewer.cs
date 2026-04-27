using UnityEngine;
using UnityEngine.UI;

public class CalendarViewer : MonoBehaviour
{
    public Image displayImage;
    public Sprite[] allMonths;
    public GameObject uiPanel;

    private int currentIndex = 0;

    public void OpenCalendar()
    {
        uiPanel.SetActive(true);
        currentIndex = 0;
        UpdateDisplay();
    }

    public void NextMonth()
    {
        if (currentIndex < allMonths.Length - 1)
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
        UpdateDisplay();
    }

    public void PreviousMonth()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
        }
        else
        {
            currentIndex = allMonths.Length - 1;
        }
        UpdateDisplay();
    }

    public void CloseCalendar()
    {
        uiPanel.SetActive(false);
    }

    private void UpdateDisplay()
    {
        displayImage.sprite = allMonths[currentIndex];
    }
}