using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTurret : Turret, IPlaceable
{
    public Transform crossbow;

    Transform IPlaceable.transform { get { return transform; } }

    protected override float range { get; set; } = 10f;
    protected override float turnSpeed { get; set; } = 4f;

    private float accuracyTolerance = 1f;

    private Transform projectilePrefab;
    private readonly float attackCooldownInSeconds = 1.5f;
    private float lastTimeAttacked = 0;

    public void Start()
    {
        projectilePrefab = Resources.Load<Transform>("ArrowProjectile");

        // since the turrets are placed by the player it should not be necessary to stagger the execution of findTarget as the player already adds randomness to it. hopefully
        InvokeRepeating("findTarget", 0f, 0.2f);
    }

    protected override void idle()
    {
        if(target != null) 
        {
            state = TurretState.AIMING;
            return;
        }

        crossbow.Rotate(0, Time.deltaTime * 20, 0);
    }

    protected override void aim()
    {
        if (target == null)
        {
            state = TurretState.IDLING;
            return;
        }

        Vector3 directionToTarget = target.position - crossbow.position;

        float angleToTarget = Vector3.Angle(crossbow.forward, directionToTarget);

        if (angleToTarget <= accuracyTolerance)
        {
            state = TurretState.SHOOTING;
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        crossbow.rotation = Quaternion.Lerp(crossbow.rotation, targetRotation, turnSpeed * Time.deltaTime);

        shoot();
    }

    protected override void shoot()
    {
        if (target == null)
        {
            state = TurretState.IDLING;
            return;
        }

        Vector3 directionToTarget = target.position - crossbow.position;
        float angleToTarget = Vector3.Angle(crossbow.forward, directionToTarget);

        if (angleToTarget > accuracyTolerance) state = TurretState.AIMING;

        if (Time.time - lastTimeAttacked < attackCooldownInSeconds) return;

        lastTimeAttacked = Time.time;

        Instantiate(projectilePrefab, crossbow.position, crossbow.rotation);
    }
}
