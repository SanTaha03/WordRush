using UnityEngine;

public class MageAimController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Correction visuelle")]
    [SerializeField] private float yRotationOffset = 0f;

    private Quaternion targetRotation;
    private bool hasTargetRotation = false;

    private void Update()
    {
        if (!hasTargetRotation)
        {
            return;
        }

        objectToRotate.rotation = Quaternion.Slerp(
            objectToRotate.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void LookAtEnemy(Enemy targetEnemy)
    {
        if (targetEnemy == null)
        {
            return;
        }

        if (objectToRotate == null)
        {
            objectToRotate = transform;
        }

        Vector3 direction = targetEnemy.transform.position - objectToRotate.position;

        // On bloque la rotation verticale pour que le mage tourne seulement sur lui-même
        direction.y = 0f;

        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion offsetRotation = Quaternion.Euler(0f, yRotationOffset, 0f);

        targetRotation = lookRotation * offsetRotation;
        hasTargetRotation = true;
    }
}