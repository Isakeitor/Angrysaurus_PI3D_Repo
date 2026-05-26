using UnityEngine;
using UnityEngine.AI;

public class EnemyIA : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform target;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animator;

    EnemyHealth health; // 👈 AÑADIDO

    [Header("Patrol")]
    [SerializeField] float walkPointRange = 8f;

    [Header("Detection")]
    [SerializeField] float sightRange = 10f;
    [SerializeField] float attackRange = 5f;

    [Header("Attack")]
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float shootForce = 20f;

    [Header("VFX")]
    [SerializeField] GameObject muzzleVFX;

    Vector3 walkPoint;

    bool walkPointSet;
    bool alreadyAttacked;

    bool targetInSightRange;
    bool targetInAttackRange;

    void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        health = GetComponent<EnemyHealth>(); // 👈 AÑADIDO

        if (target == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                target = player.transform;
        }
    }

    void Update()
    {
        if (target == null)
            return;

        // ❗ BLOQUEO SI ESTÁ MUERTO
        if (health != null && health.enabled == false)
            return;

        UpdateDetection();
        UpdateAnimator();

        if (!targetInSightRange && !targetInAttackRange)
        {
            Patroling();
        }
        else if (targetInSightRange && !targetInAttackRange)
        {
            ChasePlayer();
        }
        else if (targetInSightRange && targetInAttackRange)
        {
            AttackPlayer();
        }
    }

    void UpdateDetection()
    {
        float distance =
            Vector3.Distance(transform.position, target.position);

        targetInSightRange = distance <= sightRange;
        targetInAttackRange = distance <= attackRange;
    }

    void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint =
            transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 randomPoint =
            new Vector3(
                transform.position.x + randomX,
                transform.position.y,
                transform.position.z + randomZ
            );

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    void ChasePlayer()
    {
        walkPointSet = false;
        agent.SetDestination(target.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);

            transform.rotation =
                Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);
        }

        if (!alreadyAttacked)
        {
            Shoot();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || shootPoint == null)
            return;

        if (muzzleVFX != null)
        {
            GameObject vfx =
                Instantiate(muzzleVFX, shootPoint.position, shootPoint.rotation);

            Destroy(vfx, 2f);
        }

        GameObject bullet =
            Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
            rb.linearVelocity = shootPoint.forward * shootForce;
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void UpdateAnimator()
    {
        if (animator == null)
            return;

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}