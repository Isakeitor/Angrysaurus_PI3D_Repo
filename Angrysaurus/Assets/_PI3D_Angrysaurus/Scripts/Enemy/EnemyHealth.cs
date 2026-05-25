using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHealth = 50;

    [Header("References")]
    [SerializeField] Animator animator;
    [SerializeField] SkinnedMeshRenderer enemyRenderer;

    [Header("Damage Feedback")]
    [SerializeField] Material damageMaterial;
    [SerializeField] float flashDuration = 0.1f;

    [Header("Death")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float destroyDelay = 3f;

    int currentHealth;

    bool isDead;

    Material originalMaterial;

    void Start()
    {
        currentHealth = maxHealth;

        if (enemyRenderer != null)
        {
            originalMaterial =
                enemyRenderer.material;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        DamageFlash();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void DamageFlash()
    {
        if (enemyRenderer == null)
            return;

        if (damageMaterial == null)
            return;

        enemyRenderer.material = damageMaterial;

        CancelInvoke(nameof(ResetMaterial));

        Invoke(
            nameof(ResetMaterial),
            flashDuration
        );
    }

    void ResetMaterial()
    {
        if (enemyRenderer == null)
            return;

        enemyRenderer.material =
            originalMaterial;
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        if (deathVFX != null)
        {
            Instantiate(
                deathVFX,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject, destroyDelay);
    }
}