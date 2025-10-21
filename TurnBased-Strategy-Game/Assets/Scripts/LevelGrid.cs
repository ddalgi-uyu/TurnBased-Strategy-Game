using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    [SerializeField] private Transform gridDebugPrefab;

    private GridSystem gridSystem;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;

        gridSystem = new GridSystem(10, 10, 2);
        gridSystem.CreateDebugObject(gridDebugPrefab);
    }

    public void AddUnitAtGridPosition(GridPosition position, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        gridObject.AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition position)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        return gridObject.GetUnitList();
    }

    public void RemoveUnitAtGridPostition(GridPosition position, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(position);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPostition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);
    }

    /// <summary>
    /// Get grid position from the world grid position
    /// </summary>
    /// <param name="worldGridPosition"></param>
    /// <returns></returns>
    public GridPosition GetGridPosition(Vector3 worldGridPosition)
    {
        return gridSystem.GetGridPosition(worldGridPosition);
    }
}
