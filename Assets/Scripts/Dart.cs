using UnityEngine;
using UnityEngine.InputSystem;

public class Dart : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float dartRepositioningSpeed = 5f;
    [SerializeField] private Vector3 dartForce;

    private Camera m_mainCamera;
    
    private Rigidbody m_rigidbody;
    private bool m_alreadyLaunched;
    
    private Vector3 m_startPos;
    private Quaternion m_startRot;
    
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_mainCamera = Camera.main;
        transform.GetLocalPositionAndRotation(out m_startPos, out m_startRot);
        
        RestoreDart();
    }

    private void FixedUpdate()
    {
        if(m_alreadyLaunched)
            return;

        Vector2 cursorPos = Vector2.zero;

        switch (gameManager.CurrentControlScheme)
        {
            case "Keyboard&Mouse":
                cursorPos = Mouse.current.position.value;
                break;
            
            case "Gamepad":
                cursorPos = gameManager.gamepadPos;
                break;
        }
        
        Vector3 worldPosition = m_mainCamera.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, m_startPos.z));
        
        Vector3 lerpedPosition = Vector3.Lerp(m_rigidbody.position, worldPosition, dartRepositioningSpeed * Time.deltaTime);
        m_rigidbody.MovePosition(lerpedPosition);
    }

    public void LaunchDart()
    {
        if(m_alreadyLaunched)
            return;
        
        m_alreadyLaunched = true;
        
        m_rigidbody.useGravity = true;
        m_rigidbody.AddForce(dartForce);
    }

    public void StopDart()
    {
        m_rigidbody.useGravity = false;
        m_rigidbody.linearVelocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
    }

    public void RestoreDart()
    {
        StopDart();
        
        m_rigidbody.Move(m_startPos, m_startRot);

        m_alreadyLaunched = false;
    }
}
