using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform shootPoint;
    [SerializeField] Camera mainCamera;

    [Header("Projectile")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 25f;

    [Header("Shoot")]
    [SerializeField] float shootCooldown = 0.2f;

    [Header("VFX")]
    [SerializeField] GameObject muzzleVFX;

    [Header("Camera Feedback")]
    [SerializeField] float normalFOV = 60f;
    [SerializeField] float shootFOV = 54f;
    [SerializeField] float zoomSpeed = 12f;
    [SerializeField] float zoomReturnSpeed = 8f;

    bool canShoot = true;
    float targetFOV;

    Collider playerCollider;
    PlayerMovement movement;

    // 🔥 NUEVO
    PlayerInteraction interaction;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        playerCollider = GetComponent<Collider>();

        movement = GetComponent<PlayerMovement>();

        // 🔥 NUEVO
        interaction = GetComponent<PlayerInteraction>();

        targetFOV = normalFOV;

        mainCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        mainCamera.fieldOfView =
            Mathf.Lerp(
                mainCamera.fieldOfView,
                targetFOV,
                zoomSpeed * Time.deltaTime
            );

        targetFOV =
            Mathf.Lerp(
                targetFOV,
                normalFOV,
                zoomReturnSpeed * Time.deltaTime
            );
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!canShoot)
            return;

        // 🔥 NO DISPARAR SI LLEVA ITEM
        if (
            interaction != null &&
            interaction.IsCarrying
        )
        {
            return;
        }

        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        canShoot = false;

        Shoot();

        yield return new WaitForSeconds(shootCooldown);

        canShoot = true;
    }

    void Shoot()
    {
        targetFOV = shootFOV;

        Ray ray =
            mainCamera.ViewportPointToRay(
                new Vector3(0.5f, 0.5f, 0f)
            );

        Vector3 targetPoint;

        if (
            Physics.Raycast(
                ray,
                out RaycastHit hit
            )
        )
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 direction =
            (
                targetPoint -
                shootPoint.position
            ).normalized;

        // ROTAR PLAYER HACIA DISPARO
        if (movement != null)
        {
            movement.ForceLookDirection(direction);
        }

        // VFX
        if (muzzleVFX != null)
        {
            GameObject muzzle =
                Instantiate(
                    muzzleVFX,
                    shootPoint.position,
                    Quaternion.LookRotation(direction)
                );

            Destroy(muzzle, 2f);
        }

        // BALA
        GameObject bullet =
            Instantiate(
                bulletPrefab,
                shootPoint.position,
                Quaternion.LookRotation(direction)
            );

        // IGNORAR PLAYER
        Collider bulletCollider =
            bullet.GetComponent<Collider>();

        if (
            bulletCollider != null &&
            playerCollider != null
        )
        {
            Physics.IgnoreCollision(
                bulletCollider,
                playerCollider
            );
        }

        // MOVIMIENTO BALA
        Rigidbody rb =
            bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                direction * bulletSpeed;
        }
    }
}