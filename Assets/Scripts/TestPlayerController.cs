using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float testMoveSpeed = 5f;
    [SerializeField] private float testRunSpeed = 8f;
    [SerializeField] private float testJumpForce = 5f;
    
    [Header("Mouse Look Settings")]
    [SerializeField] private float testMouseSensitivity = 2f;
    [SerializeField] private float testMaxLookAngle = 80f;
    
    [Header("References")]
    [SerializeField] private Transform testCameraTransform;
    [SerializeField] private CharacterController testCharacterController;
    
    // Private variables
    private float testVerticalRotation = 0f;
    private Vector3 testPlayerVelocity;
    private bool testIsGrounded;
    private float testGravity = -9.81f;
    
    // Input variables
    private float testMoveForward;
    private float testMoveRight;
    private float testMouseX;
    private float testMouseY;
    private bool testIsRunning;
    private bool testIsJumping;
    
    void Start()
    {
        // Lock cursor to center of screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Get components if not assigned
        if (testCharacterController == null)
            testCharacterController = GetComponent<CharacterController>();
            
        if (testCameraTransform == null)
            testCameraTransform = Camera.main.transform;
    }
    
    void Update()
    {
        GetInput();
        HandleMouseLook();
        HandleMovement();
    }
    
    void GetInput()
    {
        // Movement input
        testMoveForward = Input.GetAxis("Vertical");
        testMoveRight = Input.GetAxis("Horizontal");
        
        // Mouse input
        testMouseX = Input.GetAxis("Mouse X") * testMouseSensitivity;
        testMouseY = Input.GetAxis("Mouse Y") * testMouseSensitivity;
        
        // Running input
        testIsRunning = Input.GetKey(KeyCode.LeftShift);
        
        // Jump input
        testIsJumping = Input.GetKeyDown(KeyCode.Space);
        
        // Cursor unlock (ESC key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        // Cursor lock (Left mouse button)
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    void HandleMouseLook()
    {
        // Horizontal rotation (player body)
        transform.Rotate(Vector3.up * testMouseX);
        
        // Vertical rotation (camera only)
        testVerticalRotation -= testMouseY;
        testVerticalRotation = Mathf.Clamp(testVerticalRotation, -testMaxLookAngle, testMaxLookAngle);
        testCameraTransform.localRotation = Quaternion.Euler(testVerticalRotation, 0f, 0f);
    }
    
    void HandleMovement()
    {
        // Check if grounded
        testIsGrounded = testCharacterController.isGrounded;
        
        if (testIsGrounded && testPlayerVelocity.y < 0)
        {
            testPlayerVelocity.y = -2f; // Small negative value to keep grounded
        }
        
        // Calculate movement direction
        Vector3 testMoveDirection = transform.right * testMoveRight + transform.forward * testMoveForward;
        
        // Apply movement speed
        float testCurrentSpeed = testIsRunning ? testRunSpeed : testMoveSpeed;
        
        // Move the character
        testCharacterController.Move(testMoveDirection * testCurrentSpeed * Time.deltaTime);
        
        // Handle jumping
        if (testIsJumping && testIsGrounded)
        {
            testPlayerVelocity.y = Mathf.Sqrt(testJumpForce * -2f * testGravity);
        }
        
        // Apply gravity
        testPlayerVelocity.y += testGravity * Time.deltaTime;
        testCharacterController.Move(testPlayerVelocity * Time.deltaTime);
    }
    
    // Optional: Add this method to reset player position (for testing)
    public void ResetPlayerPosition(Vector3 newPosition)
    {
        testCharacterController.enabled = false;
        transform.position = newPosition;
        testCharacterController.enabled = true;
    }
    
    // Optional: Add this method to change mouse sensitivity
    public void SetMouseSensitivity(float newSensitivity)
    {
        testMouseSensitivity = newSensitivity;
    }
} 