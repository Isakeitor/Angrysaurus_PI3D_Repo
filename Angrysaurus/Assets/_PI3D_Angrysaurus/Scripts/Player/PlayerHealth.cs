using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHealth = 100;

    [Header("UI Hearts")]
    [SerializeField] Image[] hearts;

    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] GameObject defeatPanel;

    int currentHealth;

    bool isDead;

    Rigidbody rb;

    PlayerMovement movement;

    void Start()
    {
        currentHealth = maxHealth;

        rb = GetComponent<Rigidbody>();

        movement =
            GetComponent<PlayerMovement>();

        if (defeatPanel != null)
        {
            defeatPanel.SetActive(false);
        }

        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log(
            "Player HP: " +
            currentHealth
        );

        UpdateHearts();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = false;
        }

        if (currentHealth > 66)
        {
            hearts[0].enabled = true;
            hearts[1].enabled = true;
            hearts[2].enabled = true;
        }
        else if (currentHealth > 33)
        {
            hearts[0].enabled = true;
            hearts[1].enabled = true;
        }
        else if (currentHealth > 0)
        {
            hearts[0].enabled = true;
        }
    }

    void Die()
    {
        isDead = true;

        // PARAR MOVIMIENTO
        if (movement != null)
        {
            movement.enabled = false;
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

        // 🔥 AVISAR AL GAMEMANAGER
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Defeat();
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