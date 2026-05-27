using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Distance")]
    [SerializeField] float distance = 5f;

    [SerializeField] float height = 2f;

    [Header("Mouse")]
    [SerializeField] float sensitivity = 2f;

    [Header("Smooth")]
    [SerializeField] float smoothSpeed = 12f;

    [Header("Vertical Clamp")]
    [SerializeField] float minPitch = -15f;

    [SerializeField] float maxPitch = 45f;

    [Header("Collision")]
    [SerializeField] LayerMask collisionLayers;

    float yaw;
    float pitch = 20f;

    Vector3 currentVelocity;

    bool cameraActive = true;

    // 🔥 NUEVO
    public float Yaw => yaw;

    void Start()
    {
        EnableCameraControl();
    }

    void LateUpdate()
    {
        if (!cameraActive)
            return;

        if (target == null)
            return;

        RotateCamera();
        FollowTarget();
    }

    void RotateCamera()
    {
        Vector2 mouseDelta =
            Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * sensitivity;

        pitch -= mouseDelta.y * sensitivity;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
        );

        // ❌ ELIMINADO:
        // target.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    void FollowTarget()
    {
        Quaternion rotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f
            );

        Vector3 desiredPosition =
            target.position
            - rotation * Vector3.forward * distance
            + Vector3.up * height;

        Vector3 direction =
            desiredPosition - target.position;

        if (
            Physics.Raycast(
                target.position,
                direction.normalized,
                out RaycastHit hit,
                distance,
                collisionLayers
            )
        )
        {
            desiredPosition =
                hit.point -
                direction.normalized * 0.3f;
        }

        transform.position =
            Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref currentVelocity,
                1f / smoothSpeed
            );

        transform.LookAt(
            target.position +
            Vector3.up * 1.5f
        );
    }

    public void DisableCameraControl()
    {
        cameraActive = false;

        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;
    }

    public void EnableCameraControl()
    {
        cameraActive = true;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }
}