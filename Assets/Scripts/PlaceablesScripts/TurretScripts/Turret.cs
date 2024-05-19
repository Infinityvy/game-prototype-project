using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected TurretTargetingMode targetingMode = TurretTargetingMode.NEAREST;

    protected TurretState state = TurretState.IDLING;

    public IEnemy target;

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
                throw new System.Exception("Unkown turret state.");
        }
    }

    protected void findTarget()
    {
        if (target != null) target.deathEvent.RemoveListener(removeTarget);

        switch (targetingMode)
        {
            case TurretTargetingMode.NEAREST:
                target = getNearestTarget();
                break;
            case TurretTargetingMode.STRONGEST:
                throw new System.NotImplementedException();
            case TurretTargetingMode.WEAKEST:
                throw new System.NotImplementedException();
            default:
                throw new System.Exception("Unknown turret targeting mode.");
        }

        if (target != null) target.deathEvent.AddListener(removeTarget);
    }

    protected IEnemy getNearestTarget()
    {
        List<IEnemy> enemies = EnemyDirector.instance.enemies;

        if (enemies.Count == 0) return null;

        IEnemy nearestEnemy = enemies[0];
        float shortestDistance = Vector3.Distance(transform.position, nearestEnemy.position);
        for (int i = 1; i < enemies.Count; i++)
        {
            if (enemies[i].getEnemyState() == EnemyState.SPAWNING) continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemies[i].position);
            if (distanceToEnemy < shortestDistance)
            {
                nearestEnemy = enemies[i];
                shortestDistance = distanceToEnemy;
            }
        }

        if (nearestEnemy.getEnemyState() == EnemyState.SPAWNING) return null;

        return nearestEnemy;
    }

    public void removeTarget()
    {
        target = null;
        state = TurretState.IDLING;
    }

    protected abstract void idle();

    protected abstract void aim();

    protected abstract void shoot();
}
