using System;
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
}
