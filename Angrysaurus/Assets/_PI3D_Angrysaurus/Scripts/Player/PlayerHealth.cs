using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHealth = 100;

    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerShoot playerShoot;
    [SerializeField] PlayerInteraction playerInteraction;

    [Header("UI")]
    [SerializeField] GameObject defeatPanel;

    [Header("Death")]
    [SerializeField] float defeatDelay = 3f;

    int currentHealth;

    bool isDead;

    void Start()
    {
        currentHealth = maxHealth;

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

        // ANIMACIÓN
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // DESACTIVAR MOVIMIENTO
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // DESACTIVAR DISPARO
        if (playerShoot != null)
        {
            playerShoot.enabled = false;
        }

        // DESACTIVAR INTERACCIONES
        if (playerInteraction != null)
        {
            playerInteraction.enabled = false;
        }

        // PARAR VELOCIDAD
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
        }

        // MOSTRAR DERROTA
        StartCoroutine(DefeatRoutine());
    }

    IEnumerator DefeatRoutine()
    {
        yield return new WaitForSeconds(defeatDelay);

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}