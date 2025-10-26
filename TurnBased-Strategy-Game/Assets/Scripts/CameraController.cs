using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UnityEngine.Rendering;
using static UnityEngine.GridBrushBase;

public class CameraController : MonoBehaviour
{
    private float moveSpeed = 5f;
    private float rotateSpeed = 10.0f;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    private const float MAX_FOLLOW_Y_OFFSET = 2f;
    private const float MIN_FOLLOW_Y_OFFSET = 12f;

    private Vector3 targetFollowOffset;
    CinemachineTransposer cinemachineTransposer;

    private void Start()
    {
        cinemachineTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = new Vector3(0, 0, 0);

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

        Vector3 moveVector = transform.forward * inputDirection.z + transform.right * inputDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        float rotationDirection = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationDirection = -1f;
        }

        transform.Rotate(0, rotationDirection * rotateSpeed * Time.deltaTime, 0);
    }

    private void HandleZoom()
    {
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        else if (Input.mouseScrollDelta.y <= 0)
        {
            targetFollowOffset.y += zoomAmount;
        }
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        if(zoomAmount == 1f)
        {
            return;
        }
        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }
}
