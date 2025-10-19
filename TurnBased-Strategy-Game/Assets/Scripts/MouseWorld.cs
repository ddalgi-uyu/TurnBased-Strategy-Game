using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask mousePlaneLayerMask;

    public static MouseWorld instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, mousePlaneLayerMask);
        transform.position = rayCastHit.point;
    }

    public static Vector3 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit rayCastHit, float.MaxValue, instance.mousePlaneLayerMask);

        return rayCastHit.point;
    }
}
