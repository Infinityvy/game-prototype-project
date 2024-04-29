using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected TurretTargetingMode targetingMode = TurretTargetingMode.NEAREST;

    protected TurretState state = TurretState.IDLING;

    protected IEnemy target;

    protected abstract float range { get; set; }
    protected abstract float turnSpeed { get; set; }

    private void Update()
    {
        switch (state)
        {
            case TurretState.IDLING:
                idle();
                break;
            case TurretState.AIMING:
                aim();
                break;
            case TurretState.SHOOTING:
                shoot();
                break;
            default:
                throw new Exception("Unkown turret state.");
        }
    }

    protected void findTarget()
    {
        switch (targetingMode)
        {
            case TurretTargetingMode.NEAREST:
                target = getNearestTarget();
                break;
            case TurretTargetingMode.STRONGEST:
                throw new NotImplementedException();
            case TurretTargetingMode.WEAKEST:
                throw new NotImplementedException();
            default:
                throw new Exception("Unknown turret targeting mode.");
        }
    }

    protected IEnemy getNearestTarget()
    {
        List<IEnemy> enemies = EnemyDirector.instance.enemies;

        if (enemies.Count == 0) return null;

        IEnemy nearestEnemy = enemies[0];
        float shortestDistance = Vector3.Distance(transform.position, nearestEnemy.position);
        for (int i = 1; i < enemies.Count; i++)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemies[i].position);
            if (distanceToEnemy < shortestDistance)
            {
                nearestEnemy = enemies[i];
                shortestDistance = distanceToEnemy;
            }
        }



        return nearestEnemy;
    }

    protected abstract void idle();

    protected abstract void aim();

    protected abstract void shoot();
}
