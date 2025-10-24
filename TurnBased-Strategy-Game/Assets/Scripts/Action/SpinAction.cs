using System;
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
        isActive = false;
        OnActionComplete();
    }

    public void Spin(Action onSpinComplete)
    {
        this.OnActionComplete = onSpinComplete;
        isActive = true;
    }

    public override string GetActionName()
    {
        return "Spin";
    }
}
