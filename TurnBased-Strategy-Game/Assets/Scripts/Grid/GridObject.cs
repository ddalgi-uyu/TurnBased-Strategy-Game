using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition postion;

    public GridObject(GridSystem gridSystem, GridPosition postion)
    {
        this.gridSystem = gridSystem;
        this.postion = postion;
    }

    public override string ToString()
    {
        return postion.ToString();
    }
}
