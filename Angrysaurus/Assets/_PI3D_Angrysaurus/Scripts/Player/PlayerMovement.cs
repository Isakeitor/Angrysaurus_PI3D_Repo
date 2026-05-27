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

    Vector3 forcedLookDirection;
    bool hasForcedLook;

    CameraFollow cameraFollow;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
            cameraFollow =
                mainCamera.GetComponent<CameraFollow>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 forward =
            mainCamera.transform.forward;

        Vector3 right =
            mainCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection =
            forward * moveInput.y +
            right * moveInput.x;

        moveDirection.Normalize();

        // MOVIMIENTO
        Vector3 movement =
            moveDirection * moveSpeed;

        rb.linearVelocity = new Vector3(
            movement.x,
            rb.linearVelocity.y,
            movement.z
        );

        Quaternion targetRotation;

        // 🔥 PRIORIDAD 1: DISPARO
        if (hasForcedLook)
        {
            Vector3 dir = forcedLookDirection;
            dir.y = 0f;

            targetRotation =
                Quaternion.LookRotation(dir);

            hasForcedLook = false;
        }

        // 🔥 PRIORIDAD 2: MOVIMIENTO
        else if (moveDirection.sqrMagnitude > 0.01f)
        {
            targetRotation =
                Quaternion.LookRotation(moveDirection);
        }

        // 🔥 PRIORIDAD 3: CÁMARA
        else
        {
            float yaw =
                cameraFollow != null
                ? cameraFollow.Yaw
                : transform.eulerAngles.y;

            targetRotation =
                Quaternion.Euler(
                    0f,
                    yaw,
                    0f
                );
        }

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

        // ANIMACIONES
        if (animator != null)
        {
            animator.SetFloat(
                "Speed",
                moveDirection.magnitude
            );
        }
    }

    public void ForceLookDirection(Vector3 direction)
    {
        forcedLookDirection = direction;
        hasForcedLook = true;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput =
            context.ReadValue<Vector2>();
    }
}