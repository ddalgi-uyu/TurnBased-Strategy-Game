using UnityEngine;

public class GridSystem : MonoBehaviour
{
    private int width;
    private int height;
    private int cellSize;
    private GridObject[,] gridObjectArray; 

    [SerializeField] private bool debug = false;

    public GridSystem(int width, int height, int cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridObjectArray = new GridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (debug)
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);

                GridPostion gridPosition = new GridPostion(x, z);
                gridObjectArray[x, z] = new GridObject(this, gridPosition);
            }
        }
    }

    /// <summary>
    /// Get world position from grid position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, x) * cellSize;
    }

    /// <summary>
    /// Get grid position from world position
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public GridPostion GridPosition(Vector3 worldPosition)
    {
        return new GridPostion(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.z / cellSize));
    }

    public void CreateDebugObject(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject.Instantiate(debugPrefab, GetWorldPosition(x, z), Quaternion.identity);
            }
        }
    }
}
