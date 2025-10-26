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

    public void Start(){
        gridSystemVisualSingles = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
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
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
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

    /// <summary>
    /// Update grid visual based on the current selected action
    /// </summary>
    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        ShowAllGridPositionList(selectedAction.GetValidGridActionPositionList());
    }
}
