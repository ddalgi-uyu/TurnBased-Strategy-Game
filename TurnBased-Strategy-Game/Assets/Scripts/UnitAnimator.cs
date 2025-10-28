using System;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs args)
    {
        animator.SetBool("isWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs args)
    {
        animator.SetBool("isWalking", false);
    }

    private void ShootAction_OnShoot(object sender, EventArgs args)
    {
        animator.SetTrigger("shoot");
    }
}
