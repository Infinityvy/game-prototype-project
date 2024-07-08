using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTurret : Turret, IPlaceable
{
    PlaceableType IPlaceable.type { get; set; } = PlaceableType.CROSSBOW;

    Transform IPlaceable.transform { get { return transform; } }

    public Transform crossbow;

    protected override float range { get; set; } = 15f;
    protected override float turnSpeed { get; set; } = 4f;

    private float accuracyTolerance = 0.5f;
    private float maxFiringAngle = 1f;

    private Transform projectilePrefab;
    private readonly float attackCooldownInSeconds = 1.5f;
    private float lastTimeAttacked = 0;

    public void Start()
    {
        projectilePrefab = Resources.Load<Transform>("ArrowProjectile");

        direction = (Random.Range(0f, 1f) < 0.5f ? 1 : -1);
    }

    private float timeAtLastFindTarget = 0;
    private float direction = 1;
    protected override void idle()
    {
        if(target != null) 
        {
            state = TurretState.AIMING;
            return;
        }

        if(Time.time - timeAtLastFindTarget > 0.2f)
        {
            findTarget();
            timeAtLastFindTarget = Time.time;
        }

        crossbow.Rotate(0, Time.deltaTime * 20 * direction, 0);
    }

    Vector3 targetPositionAtImpactTime = Vector3.zero;
    protected override void aim()
    {
        if (target == null)
        {
            state = TurretState.IDLING;
            crossbow.rotation = Quaternion.Euler(0, crossbow.rotation.eulerAngles.y, crossbow.rotation.eulerAngles.z);
            targetPositionAtImpactTime = transform.position;
            return;
        }

        Vector3 directionToTarget = target.position - crossbow.position;
        float projectileTravelTime = directionToTarget.magnitude / projectilePrefab.GetComponent<Projectile>().speed;
        targetPositionAtImpactTime = target.velocity * projectileTravelTime + target.position;

        directionToTarget = targetPositionAtImpactTime - crossbow.position;

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
            crossbow.rotation = Quaternion.Euler(0, crossbow.rotation.eulerAngles.y, crossbow.rotation.eulerAngles.z);
            targetPositionAtImpactTime = transform.position;
            return;
        }

        Vector3 directionToTarget = target.position - crossbow.position;

        float angleToTarget = Vector3.Angle(crossbow.forward, directionToTarget);

        if (angleToTarget > accuracyTolerance) state = TurretState.AIMING;

        if (angleToTarget > maxFiringAngle) return;
        if (Time.time - lastTimeAttacked < attackCooldownInSeconds) return;

        lastTimeAttacked = Time.time;

        Instantiate(projectilePrefab, crossbow.position, crossbow.rotation);
        AkSoundEngine.PostEvent("crossbow_shot_reload", gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(crossbow.position, crossbow.position + crossbow.forward * range);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(crossbow.position, targetPositionAtImpactTime);
    }
}
