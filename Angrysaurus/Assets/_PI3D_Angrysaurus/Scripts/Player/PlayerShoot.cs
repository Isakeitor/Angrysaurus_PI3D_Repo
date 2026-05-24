using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform shootPoint;

    [Header("Projectile")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 25f;

    [Header("Shooting")]
    [SerializeField] float shootCooldown = 0.2f;

    [Header("VFX")]
    [SerializeField] GameObject muzzleVFX;

    [Header("Camera FX")]
    [SerializeField] float normalFOV = 60f;
    [SerializeField] float shootFOV = 55f;
    [SerializeField] float zoomSpeed = 15f;
    [SerializeField] float zoomDuration = 0.08f;

    [Header("Post Processing")]
    [SerializeField] Volume shootVolume;
    [SerializeField] float volumeIntensity = 1f;
    [SerializeField] float volumeFadeSpeed = 10f;

    bool canShoot = true;

    float targetFOV;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Start()
    {
        targetFOV = normalFOV;

        mainCamera.fieldOfView = normalFOV;

        if (shootVolume != null)
            shootVolume.weight = 0f;
    }

    void Update()
    {
        mainCamera.fieldOfView = Mathf.Lerp(
            mainCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );

        if (shootVolume != null)
        {
            shootVolume.weight = Mathf.Lerp(
                shootVolume.weight,
                0f,
                Time.deltaTime * volumeFadeSpeed
            );
        }
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
        // MUZZLE FLASH
        if (muzzleVFX != null)
        {
            GameObject muzzle = Instantiate(
                muzzleVFX,
                shootPoint.position,
                shootPoint.rotation
            );

            Destroy(muzzle, 2f);
        }

        // CAMERA FX
        StartCoroutine(ShootZoom());

        if (shootVolume != null)
        {
            shootVolume.weight = volumeIntensity;
        }

        // BULLET
        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity =
                shootPoint.forward * bulletSpeed;
        }
    }

    IEnumerator ShootZoom()
    {
        targetFOV = shootFOV;

        yield return new WaitForSeconds(zoomDuration);

        targetFOV = normalFOV;
    }

    #region INPUT

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!canShoot)
            return;

        StartCoroutine(ShootRoutine());
    }

    #endregion
}