using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerControllerFPS : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private Health health;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private Vector2 moveInput;
    private float verticalRotation;
    private bool jumpRequested;
    private bool hasDied;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        health = GetComponent<Health>();

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        lookAction = new InputAction("Look", InputActionType.Value, "<Mouse>/delta");
        jumpAction = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDestroy()
    {
        moveAction.Dispose();
        lookAction.Dispose();
        jumpAction.Dispose();
    }

    private void Update()
    {
        if (health != null && health.IsDead && !hasDied)
        {
            hasDied = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        moveInput = moveAction.ReadValue<Vector2>();

        Vector2 lookInput = lookAction.ReadValue<Vector2>();
        RotatePlayer(lookInput);

        if (jumpAction.WasPressedThisFrame())
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        if (health != null && health.IsDead)
        {
            return;
        }

        MovePlayer();
        HandleJump();
    }

    private void MovePlayer()
    {
        Vector3 moveDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 targetVelocity = new Vector3(
            moveDirection.x * moveSpeed,
            currentVelocity.y,
            moveDirection.z * moveSpeed
        );

        rb.linearVelocity = targetVelocity;
    }

    private void HandleJump()
    {
        if (!jumpRequested)
        {
            return;
        }

        jumpRequested = false;

        if (!IsGrounded())
        {
            return;
        }

        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void RotatePlayer(Vector2 lookInput)
    {
        transform.Rotate(Vector3.up * lookInput.x * 0.1f);

        verticalRotation -= lookInput.y * 0.1f;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);

        if (cameraPivot != null)
        {
            cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }

    private bool IsGrounded()
    {
        float rayDistance = capsuleCollider.bounds.extents.y + groundCheckDistance;
        return Physics.Raycast(
            capsuleCollider.bounds.center,
            Vector3.down,
            rayDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );
    }
}