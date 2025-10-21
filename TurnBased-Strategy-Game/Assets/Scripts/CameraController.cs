using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float rotateSpeed = 1.0f;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    private const float MAX_FOLLOW_Y_OFFSET = 2f;
    private const float MIN_FOLLOW_Y_OFFSET = 12f;

    private void Update()
    {
        Vector3 inputDirection = new Vector3(0, 0, 0);
        float rotationDirection = 0;

        if (Input.GetKey(KeyCode.W))
        {
            inputDirection.z += 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            inputDirection.z -= 1f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputDirection.x -= 1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            inputDirection.x += 1f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rotationDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationDirection = -1f;
        }

        Vector3 moveVector = transform.forward * inputDirection.z + transform.right * inputDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
        transform.Rotate(0, rotationDirection * rotateSpeed * Time.deltaTime, 0);

        CinemachineTransposer cinemachineTransposer = cinemachineCamera.GetComponent<CinemachineTransposer>();
        Vector3 followOffset = cinemachineTransposer.m_FollowOffset;

        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0)
        {
            followOffset.y -= zoomAmount;
        }
        else if(Input.mouseScrollDelta.y <= 0)
        {
            followOffset.y += zoomAmount;
        }

        followOffset.y = Mathf.Clamp(followOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = followOffset;
    }
}
