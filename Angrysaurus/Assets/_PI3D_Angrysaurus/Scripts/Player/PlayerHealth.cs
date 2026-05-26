using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHealth = 100;

    [Header("References")]
    [SerializeField] Animator animator;

    [SerializeField] GameObject defeatPanel;

    int currentHealth;

    bool isDead;

    Rigidbody rb;

    PlayerMovement movement;

    PlayerShoot shoot;

    PlayerInput playerInput;

    CameraFollow cameraFollow;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody>();

        movement =
            GetComponent<PlayerMovement>();

        shoot =
            GetComponent<PlayerShoot>();

        playerInput =
            GetComponent<PlayerInput>();

        if (Camera.main != null)
        {
            cameraFollow =
                Camera.main.GetComponent<CameraFollow>();
        }

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // DESACTIVAR INPUT
        if (playerInput != null)
        {
            playerInput.DeactivateInput();
        }

        // DESACTIVAR MOVIMIENTO
        if (movement != null)
        {
            movement.enabled = false;
        }

        // DESACTIVAR SHOOT
        if (shoot != null)
        {
            shoot.enabled = false;
        }

        // DESACTIVAR CAMERA
        if (cameraFollow != null)
        {
            cameraFollow.DisableCameraControl();
        }

        // PARAR FÍSICAS
        if (rb != null)
        {
            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;

            rb.isKinematic = true;
        }

        // CURSOR
        Cursor.lockState =
            CursorLockMode.None;

        Cursor.visible = true;

        // ANIMACIÓN
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        StartCoroutine(DefeatRoutine());
    }

    IEnumerator DefeatRoutine()
    {
        yield return new WaitForSeconds(3f);

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }
    }
}