using UnityEngine;

/// <summary>
/// Common contract for objects the player can interact with using the dedicated
/// interaction input instead of click-to-interact triggers.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Called when the player activates the interact input while this object is
    /// the current target.
    /// </summary>
    /// <param name="context">Information about the player initiating the interaction.</param>
    void OnInteract(PlayerContext context);

    /// <summary>
    /// Notifies the interactable that the player entered its focus range.
    /// </summary>
    /// <param name="context">Information about the player entering focus.</param>
    void OnFocus(PlayerContext context);

    /// <summary>
    /// Notifies the interactable that the player left its focus range.
    /// </summary>
    /// <param name="context">Information about the player leaving focus.</param>
    void OnBlur(PlayerContext context);
}
