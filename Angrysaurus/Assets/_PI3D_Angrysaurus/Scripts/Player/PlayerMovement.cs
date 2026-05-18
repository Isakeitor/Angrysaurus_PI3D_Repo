using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float rotationSpeed = 15f;

    [Header("References")]
    [SerializeField] Camera mainCamera;

    Rigidbody rb;

    Vector2 moveInput;

    public Vector3 AimDirection { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        RotateToMouse();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        Vector3 velocity = moveDirection * moveSpeed;

        rb.linearVelocity = new Vector3(
            velocity.x,
            rb.linearVelocity.y,
            velocity.z
        );
    }

    void RotateToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        Plane groundPlane = new Plane(
            Vector3.up,
            transform.position
        );

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(distance);

            Vector3 direction =
                mouseWorldPosition - transform.position;

            direction.y = 0f;

            AimDirection = direction;

            if (AimDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(AimDirection);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    #region INPUT SYSTEM

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    #endregion
}