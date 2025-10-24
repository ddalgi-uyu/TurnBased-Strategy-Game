using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingles;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    public void Strart(){
        gridSystemVisualSingles = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); x++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform =  Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingles[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); x++)
            {
                gridSystemVisualSingles[x, z].Hide();
            }
        }
    }

    public void ShowAllGridPositionList(List<GridPosition> gridPositionList) {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingles[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        ShowAllGridPositionList(selectedUnit.GetMoveAction().GetValidGridActionPositionList());
    }
}
