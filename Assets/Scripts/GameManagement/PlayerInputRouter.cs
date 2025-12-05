using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Centralizes input action enabling for the player. The Player map is enabled
/// while UI maps remain available but disabled so UI systems can opt-in when needed.
/// </summary>
public class PlayerInputRouter : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public static PlayerInputRouter Instance { get; private set; }

    public static event Action<PlayerInputRouter> OnInstanceChanged;

    [Tooltip("Enable the Player action map on start (UI map stays disabled).")]
    [SerializeField] private bool autoEnablePlayer = true;

    private InputSystem_Actions inputActions;

    public event Action InteractPerformed;
    public event Action<Vector2> MoveVectorChanged;

    private Vector2 cachedMoveInput = Vector2.zero;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeActions();
        OnInstanceChanged?.Invoke(this);
    }

    private void OnEnable()
    {
        if (autoEnablePlayer)
        {
            EnablePlayerMap();
        }
    }

    private void OnDisable()
    {
        DisablePlayerMap();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            OnInstanceChanged?.Invoke(null);
        }

        if (inputActions != null)
        {
            inputActions.Player.SetCallbacks(null);
            inputActions.Dispose();
            inputActions = null;
        }
    }

    private void InitializeActions()
    {
        if (inputActions != null)
            return;

        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);

        // Keep UI actions available but disabled unless explicitly enabled elsewhere.
        inputActions.UI.Disable();
    }

    public void EnablePlayerMap()
    {
        InitializeActions();
        inputActions.Player.Enable();
    }

    public void DisablePlayerMap()
    {
        if (inputActions == null) return;
        inputActions.Player.Disable();
    }

    public void TriggerInteract()
    {
        InteractPerformed?.Invoke();
    }

    /// <summary>
    /// Returns the last move vector received from the input system.
    /// </summary>
    public Vector2 LastMoveVector => cachedMoveInput;

    public InputSystem_Actions PlayerActions => inputActions;

    #region IPlayerActions implementation
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            value = Vector2.zero;
        }

        if (cachedMoveInput != value)
        {
            cachedMoveInput = value;
            MoveVectorChanged?.Invoke(cachedMoveInput);
        }
    }
    public void OnLook(InputAction.CallbackContext context) { }
    public void OnAttack(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TriggerInteract();
        }
    }
    public void OnCrouch(InputAction.CallbackContext context) { }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnPrevious(InputAction.CallbackContext context) { }
    public void OnNext(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }
    #endregion
}
