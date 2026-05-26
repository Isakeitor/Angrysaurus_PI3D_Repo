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

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        playerCollider =
            GetComponent<Collider>();

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
        // ZOOM FEEDBACK
        targetFOV = shootFOV;

        // MUZZLE VFX
        if (muzzleVFX != null)
        {
            GameObject muzzle =
                Instantiate(
                    muzzleVFX,
                    shootPoint.position,
                    shootPoint.rotation
                );

            Destroy(muzzle, 2f);
        }

        // CREAR BALA
        GameObject bullet =
            Instantiate(
                bulletPrefab,
                shootPoint.position,
                shootPoint.rotation
            );

        // IGNORAR COLISIÓN PLAYER
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

        // MOVER BALA
        Rigidbody rb =
            bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                shootPoint.forward *
                bulletSpeed;
        }
    }
}