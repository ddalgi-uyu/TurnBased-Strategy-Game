using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance;

    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Move mouse indicator to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, mousePlaneLayerMask);
        transform.position = rayCastHit.point;
    }

    /// <summary>
    /// Get position of the mouse
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, Instance.mousePlaneLayerMask);

        return rayCastHit.point;
    }
}
