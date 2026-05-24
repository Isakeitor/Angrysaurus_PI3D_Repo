using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Follow")]
    [SerializeField] float distance = 10f;
    [SerializeField] float height = 8f;
    [SerializeField] float followSmoothness = 10f;

    [Header("Rotation")]
    [SerializeField] float mouseSensitivity = 120f;
    [SerializeField] float rotationSmoothness = 10f;

    float targetYaw;
    float currentYaw;

    void LateUpdate()
    {
        if (target == null)
            return;

        RotateCamera();
        FollowTarget();
    }

    void RotateCamera()
    {
        float mouseX =
            Mouse.current.delta.ReadValue().x;

        // ROTACIÓN OBJETIVO
        targetYaw +=
            mouseX * mouseSensitivity * Time.deltaTime;

        // SUAVIZADO ROTACIÓN
        currentYaw = Mathf.Lerp(
            currentYaw,
            targetYaw,
            rotationSmoothness * Time.deltaTime
        );
    }

    void FollowTarget()
    {
        Quaternion rotation =
            Quaternion.Euler(45f, currentYaw, 0f);

        Vector3 offset =
            rotation * new Vector3(0f, 0f, -distance);

        offset += Vector3.up * height;

        Vector3 targetPosition =
            target.position + offset;

        // SUAVIZADO POSICIÓN
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSmoothness * Time.deltaTime
        );

        transform.LookAt(
            target.position + Vector3.up * 1.5f
        );
    }
}