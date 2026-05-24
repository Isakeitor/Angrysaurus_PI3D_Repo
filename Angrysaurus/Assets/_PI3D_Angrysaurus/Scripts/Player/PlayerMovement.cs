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
        // DIRECCIÓN RELATIVA A CÁMARA
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

        // ROTACIÓN PLAYER
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );
        }

        // ANIMACIONES
        if (animator != null)
        {
            animator.SetFloat(
                "Speed",
                moveDirection.magnitude
            );
        }
    }

    #region INPUT

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    #endregion
}