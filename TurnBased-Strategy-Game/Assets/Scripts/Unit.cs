using UnityEngine;

public class Unit : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed = 4f;
    private float stoppingDistance = .1f;
    private float rotationSpeed = 10f;
    [SerializeField] private Animator unityAnimator;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
            unityAnimator.SetBool("isWalking", true);
        }
        else
        {
            unityAnimator.SetBool("isWalking", true);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
