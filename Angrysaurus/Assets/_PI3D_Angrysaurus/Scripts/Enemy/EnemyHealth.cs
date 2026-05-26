using UnityEngine;
using UnityEngine.AI;

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
            originalMaterial = enemyRenderer.material;
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

        Invoke(nameof(ResetMaterial), flashDuration);
    }

    void ResetMaterial()
    {
        if (enemyRenderer == null)
            return;

        enemyRenderer.material = originalMaterial;
    }

    void Die()
    {
        isDead = true;

        // ❌ STOP IA COMPLETAMENTE
        EnemyIA ia = GetComponent<EnemyIA>();
        if (ia != null)
            ia.enabled = false;

        // ❌ STOP NAVMESH MOVEMENT
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        // ❌ STOP PHYSICS (por si acaso)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // ANIMACIÓN MUERTE
        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        // VFX MUERTE
        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
        }

        // DESTRUIR
        Destroy(gameObject, destroyDelay);
    }
}