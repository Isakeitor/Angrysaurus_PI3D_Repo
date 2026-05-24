using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform shootPoint;

    [Header("Projectile")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 25f;

    [Header("Shoot")]
    [SerializeField] float shootCooldown = 0.2f;

    [Header("VFX")]
    [SerializeField] GameObject muzzleVFX;

    bool canShoot = true;

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
        if (muzzleVFX != null)
        {
            GameObject muzzle = Instantiate(
                muzzleVFX,
                shootPoint.position,
                shootPoint.rotation
            );

            Destroy(muzzle, 2f);
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            shootPoint.position,
            shootPoint.rotation
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                shootPoint.forward * bulletSpeed;
        }
    }
}