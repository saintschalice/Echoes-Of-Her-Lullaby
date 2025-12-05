using UnityEngine;

/// <summary>
/// Carries common references about the player initiating an interaction.
/// </summary>
public struct PlayerContext
{
    public GameObject PlayerObject { get; }
    public Transform Transform { get; }
    public JoystickPlayerController Controller { get; }

    public PlayerContext(GameObject playerObject)
    {
        PlayerObject = playerObject;
        Transform = playerObject != null ? playerObject.transform : null;
        Controller = playerObject != null ? playerObject.GetComponent<JoystickPlayerController>() : null;
    }
}
