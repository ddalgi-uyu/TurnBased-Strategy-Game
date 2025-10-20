using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Transform gridDebugObjectTransform;
    [SerializeField] private bool debug;

    private GridSystem gridSystem;

    private void Start()
    {
        gridSystem = new GridSystem(10, 10, 2);
        gridSystem.CreateDebugObject(gridDebugObjectTransform);

        if(debug)
            Debug.Log(new GridPosition(5, 7));

    }

    private void Update()
    {
        if (debug) Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}
