using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmilyAppearance_Trigger : MonoBehaviour
{
    public GameObject emilySprite;       // Yung static or pathfinding Emily
    public AudioSource emilyLullaby;   // Mula sa Phase 4
    public Light2D farEndLight;        // Mula sa Phase 4

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Activate Emily (Sprite/AI)
            if (emilySprite != null) emilySprite.SetActive(true);

            // 2. Play the lullaby
            if (emilyLullaby != null) emilyLullaby.mute = false;

            // 3. Enable the flickering light
            if (farEndLight != null) farEndLight.enabled = true; // or trigger flickering script

            // 4. Disable this trigger so it only happens once
            GetComponent<Collider2D>().enabled = false;
        }
    }
}