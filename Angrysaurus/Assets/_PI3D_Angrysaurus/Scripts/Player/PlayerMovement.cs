using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float rotationSpeed = 15f;

    [Header("References")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Animator animator;

    Rigidbody rb;
    Vector2 moveInput;

    // 🔥 NUEVO: override de rotación por disparo
    Vector3 forcedLookDirection;
    bool hasForcedLook;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection =
            forward * moveInput.y +
            right * moveInput.x;

        moveDirection.Normalize();

        // MOVIMIENTO
        Vector3 movement = moveDirection * moveSpeed;

        rb.linearVelocity = new Vector3(
            movement.x,
            rb.linearVelocity.y,
            movement.z
        );

        // 🔥 ROTACIÓN CON PRIORIDAD
        Quaternion targetRotation;

        if (hasForcedLook)
        {
            // DISPARO TIENE PRIORIDAD
            Vector3 dir = forcedLookDirection;
            dir.y = 0f;

            targetRotation = Quaternion.LookRotation(dir);

            hasForcedLook = false; // se consume
        }
        else if (moveDirection.sqrMagnitude > 0.01f)
        {
            // MOVIMIENTO
            targetRotation = Quaternion.LookRotation(moveDirection);
        }
        else
        {
            // IDLE → cámara
            Vector3 camForward = mainCamera.transform.forward;
            camForward.y = 0f;

            targetRotation = Quaternion.LookRotation(camForward);
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.fixedDeltaTime
        );

        // ANIMACIONES
        if (animator != null)
        {
            animator.SetFloat("Speed", moveDirection.magnitude);
        }
    }

    // 🔥 NUEVO: llamado desde Shoot
    public void ForceLookDirection(Vector3 direction)
    {
        forcedLookDirection = direction;
        hasForcedLook = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}