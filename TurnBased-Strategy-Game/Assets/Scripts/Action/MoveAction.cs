using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxDistance = 1;
    private List<Vector3> positionList;
    private float stoppingDistance = .1f;
    private int currentPostionIndex;

    private void Update()
    {
        if (!isActive){
            return;
        }

        Vector3 targetPosition = positionList[currentPostionIndex];

        // Move unit to target position with animation
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float rotationSpeed = 10f;
            float moveSpeed = 4f;
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }
        else
        {
            currentPostionIndex++;
            if (currentPostionIndex >= positionList.Count)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);

                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLen);
        currentPostionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPositionn in pathGridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPositionn));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }


    public override List<GridPosition> GetValidGridActionPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxDistance; x <= maxDistance; x++)
        {
            for (int z = -maxDistance; z <= maxDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(unitGridPosition == testGridPosition)
                {
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!PathFinding.Instance.HasPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }

                int pathFindingDistannceMultiplier = 10;
                if(PathFinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxDistance * pathFindingDistannceMultiplier)
                {
                    // path is too long
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
