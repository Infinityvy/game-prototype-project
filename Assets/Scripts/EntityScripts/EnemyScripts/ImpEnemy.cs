using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImpEnemy : MonoBehaviour, IEnemy, IEntity
{
    // public:
    public EnemyState state;
    public UnityEvent deathEvent { get { return _deathEvent; } }
    public UnityEvent _deathEvent;

    // private:
    private readonly float maxHealth = 20f;
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

    Transform IEnemy.transform { get { return transform; } }
    Vector3 IEnemy.position { get { return transform.position; } }
    Vector3 IEnemy.velocity { get { return _velocity; } }
    private Vector3 _velocity = Vector3.zero;

    void Awake()
    {
        _deathEvent = new UnityEvent();
    }

    void Start()
    {
        state = EnemyState.SPAWNING;

        currentHealth = maxHealth;

        targetDestination = transform.position;
        player = GameObject.Find("Player").transform;
        projectilePrefab = Resources.Load<Transform>("MudProjectile");
        
        // initiating pathing after a random delay to prevent all enemies pathing in the same frame
        InvokeRepeating(nameof(findTargetDestination), Random.Range(0f, 1f), 1.0f);

        InvokeRepeating(nameof(updateTextureOrientation), Random.Range(0f, 0.2f), 0.2f);
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
                throw new System.Exception("Unknown enemy state.");
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
        float woodChance = Random.Range(0f, 1f);
        float metalChance = Random.Range(0f, 1f);
        if (woodChance < 0.30f) PlayerBuildModeState.resourceInventory.addResources(new ResourceBlock(1, 0));
        if (metalChance < 0.05f) PlayerBuildModeState.resourceInventory.addResources(new ResourceBlock(0, 1));

        EnemyDirector.instance.enemies.Remove(this);
        _deathEvent.Invoke();
        Destroy(gameObject);
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

        Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(directionToPlayer));
    }

    private void move()
    {
        if (Vector3.Distance(transform.position, targetDestination) <= toleranceDistance)
        {
            state = EnemyState.ATTACKING;
            _velocity = Vector3.zero;
            return;
        }

        _velocity = (targetDestination - transform.position).normalized * movementSpeed;

        transform.Translate(_velocity * Time.deltaTime);
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

    private int lastSide = 1;
    private void updateTextureOrientation()
    {
        int side = PlayerCamera.instance.getSideOfCamera(transform.position);

        if (side == lastSide) return;

        transform.Find("Model").localScale = new Vector3(side, 1, 1);

        lastSide = side;
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
}
