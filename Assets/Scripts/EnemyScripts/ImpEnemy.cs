using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImpEnemy : MonoBehaviour, IEnemy
{
    // public:

    // private:
    public EnemyState state;

    private readonly float maxHealth = 100f;
    private float currentHealth;

    private readonly float movementSpeed = 2f;
    private readonly float altitude = 1.6f;

    private Vector3 targetDestination;
    private Transform player;

    private readonly float toleranceDistance = 0.1f;
    private readonly float minDistanceToPlayer = 5.0f;
    private readonly float medianDistanceToPlayer = 8.0f;
    private readonly float maxDistanceToPlayer = 11.0f;

    private Transform projectilePrefab;
    private readonly float attackCooldownInSeconds = 1.5f;
    private float lastTimeAttacked = 0;

    Vector3 IEnemy.position { get { return transform.position; } }

    void Start()
    {
        state = EnemyState.SPAWNING;

        currentHealth = maxHealth;

        targetDestination = transform.position;
        player = GameObject.Find("Player").transform;
        projectilePrefab = Resources.Load<Transform>("MudProjectile");

        InvokeRepeating("findTargetDestination", 0f, 1.0f);
    }

    void Update()
    {
        switch (state) 
        {
            case EnemyState.ATTACKING:
                attack();
                break;
            case EnemyState.MOVING:
                move();
                break;
            case EnemyState.SPAWNING:
                spawn();
                break;
            default:
                throw new Exception("Unknown enemy state.");
        }
    }

    public EnemyState getEnemyState()
    {
        return state;
    }

    public void dealDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) die();
    }

    public float getHealth()
    {
        return currentHealth;
    }

    private void die()
    {
        throw new NotImplementedException();
    }

    private void findTargetDestination()
    {
        Vector2 playerPos = new Vector2(player.position.x, player.position.z);
        Vector2 impPos = new Vector2(transform.position.x, transform.position.z);

        float distance = Vector2.Distance(playerPos, impPos);

        Vector2 vectorToPlayer = playerPos - impPos;

        if (distance > maxDistanceToPlayer || distance < minDistanceToPlayer)
        {
            Vector2 vectorToDestination = vectorToPlayer.normalized * (distance - medianDistanceToPlayer);

            targetDestination = new Vector3(transform.position.x + vectorToDestination.x, transform.position.y,
                                            transform.position.z + vectorToDestination.y);
        }
    }

    private void attack()
    {
        if (Vector3.Distance(transform.position, targetDestination) > toleranceDistance)
        {
            state = EnemyState.MOVING;
            return;
        }

        if (Time.time - lastTimeAttacked < attackCooldownInSeconds) return;

        lastTimeAttacked = Time.time;

        Vector3 directionToPlayer = player.position - transform.position + Vector3.up;

        Transform projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(directionToPlayer));
    }

    private void move()
    {
        if (Vector3.Distance(transform.position, targetDestination) <= toleranceDistance)
        {
            state = EnemyState.ATTACKING;
            return;
        }
        
        transform.Translate((targetDestination - transform.position).normalized * movementSpeed * Time.deltaTime);
    }

    private void spawn()
    {
        if (altitude - transform.position.y <= toleranceDistance)
        {
            transform.position = new Vector3(transform.position.x, altitude, transform.position.z);
            state = EnemyState.ATTACKING;
            return;
        }

        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, targetDestination);
    }

    public static Transform getPrefab()
    {
        Transform prefab = Resources.Load<Transform>("ImpEnemy");

        return prefab;
    }

    EnemyState IEnemy.getEnemyState()
    {
        throw new NotImplementedException();
    }

    float IEnemy.getHealth()
    {
        throw new NotImplementedException();
    }

    void IEnemy.dealDamage(float damage)
    {
        throw new NotImplementedException();
    }
}
