using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Vector3 dirAwayFromCamera = (transform.position - cameraTransform.position).normalized;
        transform.LookAt(transform.position + dirAwayFromCamera);
    }
}
