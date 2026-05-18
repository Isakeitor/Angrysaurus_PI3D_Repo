using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] Transform firePoint;
    [SerializeField] float aimDistance = 10f;
    [SerializeField] float holdTimeToAim = 1f;

    [Header("Visual")]
    [SerializeField] LineRenderer lineRenderer;

    PlayerMovement movement;

    public bool IsAiming { get; private set; }

    bool aimHeld;
    float aimHoldTimer;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        lineRenderer.enabled = false;
    }

    private void Update()
    {
        HandleAim();

        if (IsAiming)
        {
            DrawAimLine();
        }
    }

    void HandleAim()
    {
        if (!aimHeld)
            return;

        aimHoldTimer += Time.deltaTime;

        if (aimHoldTimer >= holdTimeToAim && !IsAiming)
        {
            StartAim();
        }
    }

    void DrawAimLine()
    {
        Vector3 start = firePoint.position;

        Vector3 dir = movement.AimDirection.normalized;

        Vector3 end = start + dir * aimDistance;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    void StartAim()
    {
        IsAiming = true;
        lineRenderer.enabled = true;
    }

    public void StopAim()
    {
        IsAiming = false;

        aimHeld = false;
        aimHoldTimer = 0f;

        lineRenderer.enabled = false;
    }

    #region INPUT SYSTEM

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            aimHeld = true;
        }

        if (context.canceled)
        {
            aimHeld = false;
        }
    }

    #endregion
}