using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinegunTurret : Turret, IPlaceable
{
    protected override float range { get; set; } = 13f;
    protected override float turnSpeed { get; set; } = 4.5f;

    public Transform machinegun;
    public MeshRenderer muzzleFlash;

    PlaceableType IPlaceable.type { get; set; } = PlaceableType.MACHINEGUN;
    Transform IPlaceable.transform { get { return transform; } }

    private float accuracyTolerance = 2f;
    private float maxFiringAngle = 3f;

    private Transform projectilePrefab;
    private readonly float attackCooldownInSeconds = 0.1f;
    private float lastTimeAttacked = 0;

    private Sound[] sounds;

    void Start()
    {
        projectilePrefab = Resources.Load<Transform>("MGBulletProjectile");

        sounds = GameUtility.loadSounds("Machinegun", VolumeManager.machinegunBaseVolume, 1);

        gameObject.createAudioSources(sounds);
        VolumeManager.addEffects(sounds);
    }

    private float timeAtLastFindTarget = 0;
    private int direction = 1;
    protected override void idle()
    {
        if (target != null)
        {
            state = TurretState.AIMING;
            return;
        }

        if (Time.time - timeAtLastFindTarget > 0.2f)
        {
            findTarget();
            timeAtLastFindTarget = Time.time;
        }

        if (Random.Range(0f, 1f) < 0.2f * Time.deltaTime) direction *= -1;

        machinegun.Rotate(0, Time.deltaTime * 20 * direction, 0);
    }

    private Vector3 targetPositionAtImpactTime = Vector3.zero;
    protected override void aim()
    {
        if (target == null)
        {
            state = TurretState.IDLING;
            machinegun.rotation = Quaternion.Euler(0, machinegun.rotation.eulerAngles.y, machinegun.rotation.eulerAngles.z);
            return;
        }

        Vector3 directionToTarget = target.position - machinegun.position;
        float projectileTravelTime = directionToTarget.magnitude / projectilePrefab.GetComponent<Projectile>().speed;
        targetPositionAtImpactTime = target.velocity * projectileTravelTime + target.position;

        directionToTarget = targetPositionAtImpactTime - machinegun.position;

        float angleToTarget = Vector3.Angle(machinegun.forward, directionToTarget);

        if (angleToTarget <= accuracyTolerance)
        {
            state = TurretState.SHOOTING;
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        machinegun.rotation = Quaternion.Lerp(machinegun.rotation, targetRotation, turnSpeed * Time.deltaTime);

        shoot();
    }

    protected override void shoot()
    {
        if (target == null)
        {
            state = TurretState.IDLING;
            machinegun.rotation = Quaternion.Euler(0, machinegun.rotation.eulerAngles.y, machinegun.rotation.eulerAngles.z);
            return;
        }

        Vector3 directionToTarget = target.position - machinegun.position;

        float angleToTarget = Vector3.Angle(machinegun.forward, directionToTarget);

        if (angleToTarget > accuracyTolerance) state = TurretState.AIMING;

        if (angleToTarget > maxFiringAngle) return;
        if (Time.time - lastTimeAttacked < attackCooldownInSeconds) return;

        lastTimeAttacked = Time.time;

        Instantiate(projectilePrefab, machinegun.position + machinegun.forward, machinegun.rotation * Quaternion.Euler(Random.Range(0f, 1f), Random.Range(0f, 1f), 0));
        sounds[3].play();
        muzzleFlash.enabled = true;
        muzzleFlash.transform.Rotate(Random.Range(0f, 360f), 0, 0);
        Invoke(nameof(disableMuzzleFlash), 0.1f);
    }

    private void disableMuzzleFlash()
    {
        muzzleFlash.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(machinegun.position, machinegun.forward * range);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(machinegun.position, targetPositionAtImpactTime);
    }
}
