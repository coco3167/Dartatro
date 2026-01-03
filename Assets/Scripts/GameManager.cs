using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Dart dart;
    [SerializeField] private InputActionReference launchAction, restoreAction, moveAction;
    [SerializeField] private float gamepadSpeed;
    
    private PlayerInput m_playerInput;
    private Vector2 m_gamepadCurrentMovement;

    public Vector2 gamepadPos { get; private set; }

    public string CurrentControlScheme { get; private set; }
    
    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_playerInput.onActionTriggered += OnActionTriggered;
        m_playerInput.onControlsChanged += OnControlsChanged;

        CurrentControlScheme = m_playerInput.currentControlScheme;
        
        gamepadPos = Vector2.one/2;
    }

    private void FixedUpdate()
    {
        gamepadPos += m_gamepadCurrentMovement;
        gamepadPos = new Vector2(Mathf.Clamp01(gamepadPos.x), Mathf.Clamp01(gamepadPos.y));
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.started && obj.action == launchAction.action)
        {
            dart.LaunchDart();
            return;
        }

        if (obj.started && obj.action == restoreAction.action)
        {
            dart.RestoreDart();
            gamepadPos = Vector2.one/2;
        }

        if (obj.action == moveAction.action)
        {
            if(obj.performed)
                m_gamepadCurrentMovement = obj.ReadValue<Vector2>() * gamepadSpeed * Time.deltaTime;
            
            if(obj.canceled)
                m_gamepadCurrentMovement = Vector2.zero;
        }
    }
    
    private void OnControlsChanged(PlayerInput obj)
    {
        CurrentControlScheme = obj.currentControlScheme;
        Debug.Log(CurrentControlScheme);
    }
}
