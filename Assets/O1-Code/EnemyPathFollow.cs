using UnityEngine;

public class EnemyPathFollow : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float reachDistance = 0.1f;

    [Header("Chemin")]
    [SerializeField] private Transform[] waypoints;

    private int currentWaypointIndex = 0;

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        if (currentWaypointIndex >= waypoints.Length)
        {
            ReachGoal();
            return;
        }

        Transform targetPoint = waypoints[currentWaypointIndex];

        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);

        if (distanceToTarget <= reachDistance)
        {
            currentWaypointIndex++;
        }
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
    }

    private void ReachGoal()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife(1);
        }

        Destroy(gameObject);
    }
}