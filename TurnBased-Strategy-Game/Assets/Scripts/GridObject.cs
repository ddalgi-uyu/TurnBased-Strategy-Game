using UnityEngine;

public class GridObject
{
    private GridSystem gridSystem;
    private GridPostion postion;

    public GridObject(GridSystem gridSystem, GridPostion postion)
    {
        this.gridSystem = gridSystem;
        this.postion = postion;
    }
}
