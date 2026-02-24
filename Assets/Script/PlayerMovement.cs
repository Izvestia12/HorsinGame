using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float gravity = -9.81f;

    [Header("Look Settings")]
    public Camera playerCamera;
    public float mouseSensitivity = 2.0f;
    public float upperLookLimit = 80.0f;
    public float lowerLookLimit = -80.0f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float rotationX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float moveX = Input.GetAxis("Horizontal"); // A, D
        float moveZ = Input.GetAxis("Vertical");   // W, S

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * moveZ) + (right * moveX);
        moveDirection *= currentSpeed;

        if (!characterController.isGrounded)
        {
            moveDirection.y = movementDirectionY + (gravity * Time.deltaTime);
        }
        else
        {
            moveDirection.y = -0.5f; 
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleRotation()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0);

        rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, lowerLookLimit, upperLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}