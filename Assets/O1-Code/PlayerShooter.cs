using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private void Start()
    {
        Debug.Log($"PlayerShooter Start sur : {gameObject.name} | projectilePrefab = {(projectilePrefab != null ? projectilePrefab.name : "NULL")} | firePoint = {(firePoint != null ? firePoint.name : "NULL")}");
    }

    public void ShootAtEnemy(Enemy targetEnemy)
    {
        Debug.Log($"ShootAtEnemy appelé sur {gameObject.name}");

        if (projectilePrefab == null)
        {
            Debug.LogWarning($"PlayerShooter : aucun projectilePrefab assigné sur {gameObject.name}.");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning($"PlayerShooter : aucun firePoint assigné sur {gameObject.name}.");
            return;
        }

        if (targetEnemy == null)
        {
            Debug.LogWarning("PlayerShooter : targetEnemy est null.");
            return;
        }

        GameObject projectileInstance = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Debug.Log("Projectile instancié : " + projectileInstance.name);

        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();

        if (projectileScript == null)
        {
            Debug.LogWarning("PlayerShooter : le prefab instancié n'a pas de composant Projectile.");
            return;
        }

        projectileScript.SetTarget(targetEnemy);
    }
}