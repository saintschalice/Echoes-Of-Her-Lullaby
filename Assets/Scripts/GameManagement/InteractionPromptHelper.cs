using TMPro;
using UnityEngine;

/// <summary>
/// Lightweight helper to show and hide interaction prompts for world objects.
/// Keeps UI driving within world interactables without touching menu/inventory canvases.
/// </summary>
public static class InteractionPromptHelper
{
    /// <summary>
    /// Updates the prompt text (if provided) and shows the prompt root.
    /// </summary>
    public static void ShowPrompt(GameObject promptRoot, string message = null, TextMeshProUGUI promptLabel = null)
    {
        if (promptRoot == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(message) && promptLabel != null)
        {
            promptLabel.text = message;
        }

        if (!promptRoot.activeSelf)
        {
            promptRoot.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the prompt root if it exists.
    /// </summary>
    public static void HidePrompt(GameObject promptRoot)
    {
        if (promptRoot == null)
        {
            return;
        }

        if (promptRoot.activeSelf)
        {
            promptRoot.SetActive(false);
        }
    }

    /// <summary>
    /// Positions the prompt near a world target. Works with both screen space and world space prompts.
    /// </summary>
    public static void PositionPrompt(GameObject promptRoot, Transform worldTarget, Vector3 worldOffset, Camera cameraOverride = null)
    {
        if (promptRoot == null || worldTarget == null)
        {
            return;
        }

        RectTransform rectTransform = promptRoot.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            Camera targetCamera = cameraOverride != null ? cameraOverride : Camera.main;
            if (targetCamera != null)
            {
                Vector3 screenPosition = targetCamera.WorldToScreenPoint(worldTarget.position + worldOffset);
                rectTransform.position = screenPosition;
            }
        }
        else
        {
            promptRoot.transform.position = worldTarget.position + worldOffset;
        }
    }
}
