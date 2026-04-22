using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Réglages")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private float hitDistance = 0.4f;

    private Enemy targetEnemy;
    private Collider targetCollider;
    private bool hasHit = false;

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    private void Update()
    {
        if (hasHit)
        {
            return;
        }

        if (targetEnemy == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPoint = GetTargetPoint();
        Vector3 direction = targetPoint - transform.position;

        if (direction.magnitude <= hitDistance)
        {
            HitTarget();
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint,
            moveSpeed * Time.deltaTime
        );

        if (direction != Vector3.zero)
        {
            transform.forward = direction.normalized;
        }
    }

    public void SetTarget(Enemy enemyTarget)
    {
        targetEnemy = enemyTarget;

        if (targetEnemy != null)
        {
            targetCollider = targetEnemy.GetComponent<Collider>();
        }
    }

    private Vector3 GetTargetPoint()
    {
        if (targetCollider != null)
        {
            return targetCollider.bounds.center;
        }

        return targetEnemy.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
        {
            return;
        }

        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy == null)
        {
            return;
        }

        if (enemy != targetEnemy)
        {
            return;
        }

        HitTarget();
    }

    private void HitTarget()
    {
        if (hasHit)
        {
            return;
        }

        hasHit = true;

        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}