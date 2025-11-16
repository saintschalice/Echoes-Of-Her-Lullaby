using UnityEngine;
using UnityEngine.UI;

public class BackgroundScaler : MonoBehaviour
{
    private RawImage rawImage;

    void Start()
    {
        rawImage = GetComponent<RawImage>();

        if (rawImage == null)
        {
            Debug.LogError("No RawImage component found!");
            return;
        }

        ScaleToFit();
    }

    void ScaleToFit()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float imageAspect = 16f / 9f;

        Debug.Log($"Screen Aspect: {screenAspect}, Image Aspect: {imageAspect}");

        if (screenAspect > imageAspect)
        {
            float heightScale = screenAspect / imageAspect;
            float yOffset = (1f - (1f / heightScale)) / 2f;
            rawImage.uvRect = new Rect(0, yOffset, 1, 1f / heightScale);
            Debug.Log($"Cropping top/bottom. UV Rect: {rawImage.uvRect}");
        }
        else
        {
            float widthScale = imageAspect / screenAspect;
            float xOffset = (1f - (1f / widthScale)) / 2f;
            rawImage.uvRect = new Rect(xOffset, 0, 1f / widthScale, 1);
            Debug.Log($"Cropping left/right. UV Rect: {rawImage.uvRect}");
        }
    }
}