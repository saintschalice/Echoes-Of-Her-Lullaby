using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridScrollController : MonoBehaviour
{
    [Header("References")]
    public RectTransform contentRect; // Drag your 'Content' object here

    [Header("Settings")]
    public float scrollDuration = 0.25f; // How fast the animation is

    // Based on your Grid Layout Group screenshot:
    // Cell X (200) + Spacing X (-90) = 110 effective width per column
    public float itemWidth = 200f;
    public float spacing = -90f;

    [Tooltip("How many columns to scroll. You can use decimals (e.g., 1.5) for precise alignment.")]
    public float columnsToScroll = 1.0f; // Changed from int to float

    private float _stepSize;
    private float _targetX;
    private bool _isScrolling = false;

    void Start()
    {
        // Calculate how far to move for one "step"
        // Formula: (Cell Size + Spacing) * number of columns
        _stepSize = (itemWidth + spacing) * columnsToScroll;

        // Initialize target to current position to prevent jumping on start
        if (contentRect != null)
        {
            _targetX = contentRect.anchoredPosition.x;
        }
    }

    public void ScrollLeft()
    {
        if (_isScrolling || contentRect == null) return;

        // Moving Left logic (content moves Right/Positive X)
        // We set a limit (usually 0) so we don't scroll past the first item
        float maxLeft = 0f;

        if (_targetX < maxLeft)
        {
            _targetX += _stepSize;

            // Clamp to ensure we don't overshoot the start
            if (_targetX > maxLeft) _targetX = maxLeft;

            StartCoroutine(AnimateScroll());
        }
    }

    public void ScrollRight()
    {
        if (_isScrolling || contentRect == null) return;

        // Moving Right logic (content moves Left/Negative X)
        // Calculate max scroll based on content width vs viewport logic
        float viewportWidth = GetComponent<RectTransform>().rect.width;
        float contentWidth = contentRect.rect.width;

        // The farthest left the content can go (negative number)
        float maxScroll = -(contentWidth - viewportWidth);

        // Add a tiny buffer (0.1f) to avoid floating point errors preventing the last scroll
        if (_targetX > maxScroll + 0.1f)
        {
            _targetX -= _stepSize;

            // Clamp to ensure we don't overshoot the end
            if (_targetX < maxScroll) _targetX = maxScroll;

            StartCoroutine(AnimateScroll());
        }
    }

    IEnumerator AnimateScroll()
    {
        _isScrolling = true;
        float elapsedTime = 0f;
        float startX = contentRect.anchoredPosition.x;

        while (elapsedTime < scrollDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / scrollDuration;

            // SmoothStep ease-in/ease-out for a "premium" feel
            t = t * t * (3f - 2f * t);

            float newX = Mathf.Lerp(startX, _targetX, t);
            contentRect.anchoredPosition = new Vector2(newX, contentRect.anchoredPosition.y);

            yield return null;
        }

        // Ensure we land exactly on the target pixel
        contentRect.anchoredPosition = new Vector2(_targetX, contentRect.anchoredPosition.y);
        _isScrolling = false;
    }

    // Helper to visualize the bounds in Scene view
    void OnDrawGizmosSelected()
    {
        if (contentRect == null) return;

        // Draw where the "Step" would land from current position
        Gizmos.color = Color.green;
        Vector3 currentPos = contentRect.position;
        float stepWorld = (itemWidth + spacing) * columnsToScroll * transform.lossyScale.x; // approximate world scale

        Gizmos.DrawLine(currentPos, currentPos + Vector3.right * stepWorld);
        Gizmos.DrawLine(currentPos, currentPos + Vector3.left * stepWorld);
    }
}