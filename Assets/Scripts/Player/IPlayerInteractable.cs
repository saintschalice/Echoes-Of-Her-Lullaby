using UnityEngine;

public interface IPlayerInteractable
{
    /// <summary>
    /// Execute the interaction logic. Distance gating should be handled inside.
    /// </summary>
    void Interact();

    /// <summary>
    /// World transform for distance checks and identification.
    /// </summary>
    Transform Transform { get; }
}
