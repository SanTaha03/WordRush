using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null)
        {
            return;
        }

        Vector3 direction = transform.position - mainCamera.transform.position;
        transform.forward = direction;
    }
}