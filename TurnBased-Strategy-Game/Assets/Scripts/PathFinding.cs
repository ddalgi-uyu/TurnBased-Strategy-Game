using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private int width;
    private int height;
    private float cellSize;

    private void Awake()
    {
        new GridSystem<PathNode>(10, 10, 2,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
    }
}
