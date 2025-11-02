using System;
using System.Collections;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

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

    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs args)
    {
        // Validate prefab
        if (bulletProjectilePrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        animator.SetTrigger("shoot");

        // Instantiate
        Transform bulletTransform = Instantiate(
            bulletProjectilePrefab,
            shootPointTransform.position,
            Quaternion.identity
        );

        BulletProjectile bullet = bulletTransform.GetComponent<BulletProjectile>();

        if (bullet == null)
        {
            Debug.LogError($"BulletProjectile component missing on prefab: {bulletProjectilePrefab.name}");
            Destroy(bulletTransform.gameObject);
            return;
        }

        // Setup bullet
        Vector3 targetPosition = args.targetUnit.GetWorldPosition();
        targetPosition.y = shootPointTransform.position.y;
        bullet.Setup(targetPosition);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
    }
}
