using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    PlayerAim aim;

    private void Awake()
    {
        aim = GetComponent<PlayerAim>();
    }

    void Shoot()
    {
        Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Debug.Log("DISPARANDO");
    }

    #region INPUT SYSTEM

    public void OnAim(InputAction.CallbackContext context)
    {
        // Soltar después de apuntar
        if (context.canceled && aim.IsAiming)
        {
            Shoot();
            aim.StopAim();
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        // Click rápido
        if (context.performed && !aim.IsAiming)
        {
            Shoot();
        }
    }

    #endregion
}