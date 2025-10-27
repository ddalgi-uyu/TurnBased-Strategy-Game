using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    // public delegate void SpinCompleteDelegate();
    //SpinCompleteDelegate OnSpinComplete;

    //private Action OnSpinComplete; 

    public void Update()
    {
        if (!isActive)
        {
            return;
        }

        transform.Rotate(0, 360 * Time.deltaTime, 0);
        ActionComplete();
    }

    // When things goes complex, consider create a base class and make it as the parameter. In the function, cast it to the sub type. 
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override List<GridPosition> GetValidGridActionPositionList()
    {
        GridPosition gridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            gridPosition
        };
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
}
