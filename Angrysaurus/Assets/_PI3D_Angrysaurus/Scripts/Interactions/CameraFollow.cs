using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Follow")]
    [SerializeField] float distance = 10f;
    [SerializeField] float height = 8f;
    [SerializeField] float smoothSpeed = 10f;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 120f;

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

        currentYaw +=
            mouseX * rotationSpeed * Time.deltaTime;
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

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}