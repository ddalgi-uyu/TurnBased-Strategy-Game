using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    protected bool isActive;
    protected Unit unit;
    protected Action OnActionComplete;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPosition = GetValidGridActionPositionList();
        return validGridPosition.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidGridActionPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }
}
