using System.Collections;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Animation")]
    [SerializeField] private MageAttackAnimator mageAttackAnimator;

    [Header("Rotation")]
    [SerializeField] private MageAimController mageAimController;

    [Header("Timing par défaut")]
    [SerializeField] private float defaultProjectileDelay = 0.35f;

    private bool isShooting = false;

    public bool ShootAtEnemy(Enemy targetEnemy)
    {
        if (isShooting)
        {
            return false;
        }

        if (projectilePrefab == null)
        {
            Debug.LogWarning("PlayerShooter : aucun projectilePrefab assigné.");
            return false;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("PlayerShooter : aucun firePoint assigné.");
            return false;
        }

        if (targetEnemy == null)
        {
            Debug.LogWarning("PlayerShooter : targetEnemy est null.");
            return false;
        }

        StartCoroutine(ShootWithAnimationDelay(targetEnemy));
        return true;
    }

    private IEnumerator ShootWithAnimationDelay(Enemy targetEnemy)
    {
        isShooting = true;

        if (mageAimController != null)
        {
            mageAimController.LookAtEnemy(targetEnemy);
        }

        float delay = defaultProjectileDelay;

        if (mageAttackAnimator != null)
        {
            delay = mageAttackAnimator.PlayNextAttack();
        }

        yield return new WaitForSeconds(delay);

        if (targetEnemy != null)
        {
            SpawnProjectile(targetEnemy);
        }

        isShooting = false;
    }

    private void SpawnProjectile(Enemy targetEnemy)
    {
        GameObject projectileInstance = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        Projectile projectileScript = projectileInstance.GetComponent<Projectile>();

        if (projectileScript == null)
        {
            Debug.LogWarning("PlayerShooter : le prefab instancié n'a pas de composant Projectile.");
            return;
        }

        projectileScript.SetTarget(targetEnemy);
    }
}