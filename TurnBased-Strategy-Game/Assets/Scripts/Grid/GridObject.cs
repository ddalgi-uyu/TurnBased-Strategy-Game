using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition postion;
    private List<Unit> unitList;

    public GridObject(GridSystem gridSystem, GridPosition postion)
    {
        this.gridSystem = gridSystem;
        this.postion = postion;

        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in unitList) {
            unitString += unit + "\n";
        }

        return postion.ToString() + "\n" + unitString;
    }

    public void AddUnit(Unit unit)
    {
        this.unitList.Add(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetFirstUnit()
    {
        if (HasAnyUnit())
        {
            return unitList[0];
        }

        return null;
    }
}
