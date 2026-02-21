using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding Instance { get; private set; }
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform gridDebugPrefab;
    [SerializeField] private LayerMask obstaclesLayerMask;

    private int width;
    private int height;
    private float cellSize;
    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Setup(int width, int height, int cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        //gridSystem.CreateDebugObject(gridDebugPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                    Debug.Log($"grid is not walkable: {x} {z}");
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startPostion, GridPosition endPosition, out int pathLen)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closeList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startPostion);
        PathNode endtNode = gridSystem.GetGridObject(endPosition);
        openList.Add(startNode);

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPostion, endPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentPathNode = GetLowestFCostPathNode(openList);

            if (currentPathNode == endtNode)
            {
                pathLen = endtNode.GetFCost();
                return CalculatePath(endtNode);
            }

            openList.Remove(currentPathNode);
            closeList.Add(currentPathNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentPathNode))
            {
                if (closeList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closeList.Add(neighbourNode);
                    Debug.Log($"skip unwalkable node: {neighbourNode.ToString()}");
                    continue;
                }

                int tentativeGCost = currentPathNode.GetGCost() + CalculateDistance(currentPathNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentPathNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // No path found
        pathLen = 0;
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionB - gridPositionA;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;

    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z));

            if (gridPosition.z - 1 >= 0)
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));

            if (gridPosition.z + 1 < gridSystem.GetHeight())
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z));

            if (gridPosition.z - 1 >= 0)
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));

            if (gridPosition.z + 1 < gridSystem.GetHeight())
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
        }

        if (gridPosition.z - 1 >= 0)
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z - 1));

        if (gridPosition.z + 1 < gridSystem.GetHeight())
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z + 1));

        return neighbourList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNdoe = endNode;
        while (currentNdoe.GetCameFromePathNode() != null)
        {
            pathNodeList.Add(currentNdoe.GetCameFromePathNode());
            currentNdoe = currentNdoe.GetCameFromePathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPostionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPostionList.Add(pathNode.GetGridPosition());
        }

        return gridPostionList;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLen) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLen);
        return pathLen;
    }
}


