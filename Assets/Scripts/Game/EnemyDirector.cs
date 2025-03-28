using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    public static EnemyDirector instance {  get; private set; }

    public List<IEnemy> enemies = new List<IEnemy>();
    public bool paused = false;

    // spawn variables
    private static readonly float spawnAreaThickness = 4f * TileBuilder.tileSize;
    private static readonly float innerSpawnSize = (TileBuilder.maxSize + 1) * TileBuilder.tileSize;
    private static readonly float outerSpawnSize = innerSpawnSize + 2 * spawnAreaThickness;

    private float minSpawnInterval = 20f;
    private float maxSpawnInterval = 35f;
    private float averageSpawnInterval;
    private float lastSpawnInterval;

    private int waveCount = 0;

    void Start()
    {
        instance = this;

        averageSpawnInterval = minSpawnInterval + (maxSpawnInterval - minSpawnInterval) * 0.5f;
        lastSpawnInterval = averageSpawnInterval;

        Invoke(nameof(spawnWave), 0.1f);

    }

    void Update()
    {
        
    }

    private void spawnWave()
    {
        if(paused) return;

        waveCount++;

        int waveBalance = 1 + waveCount;

        // every 5 waves, the balance is doubled; however since every 10th wave spawns nothing, only every other 10th wave is doubled
        float multiplierBalance = (waveCount % 5 == 0 ? 2f : 1f);

        int balance = (int)(waveBalance * multiplierBalance);

        // every 10 waves, no enemies spawn to give player a break
        balance *= (waveCount % 10 == 0 ? 0 : 1);

        for(int i = 0; i < balance; i++)
        {
            Invoke(nameof(spawnImp), Random.Range(0f, 1f));
        }

        float randomSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        float spawnInterval = randomSpawnInterval + averageSpawnInterval - lastSpawnInterval;

        spawnInterval *= (waveCount < 3 ? 0.75f : 1);

        Debug.Log("Wave: " + waveCount + " || Balance: " + balance + " || Next wave in: " + spawnInterval + " seconds");

        Invoke(nameof(spawnWave), spawnInterval);

        lastSpawnInterval = randomSpawnInterval;
    }

    private void spawnImp() { spawnEnemy(EnemyType.IMP); } 
    private void spawnEnemy(EnemyType type)
    {
        switch (type) 
        {
            case EnemyType.IMP:
                enemies.Add(Instantiate(ImpEnemy.getPrefab(), findSpawnLocation(), Quaternion.identity).GetComponent<ImpEnemy>());
                break;
            default:
                throw new System.Exception("Unkown EnemyType");
        }
    }

    private Vector3 findSpawnLocation()
    {
        int side = Random.Range(0, 4);

        float randomOffset = Random.Range(0, spawnAreaThickness);
        float randomCoordiante = Random.Range(-outerSpawnSize / 2, outerSpawnSize / 2);
        Vector3 randomLocation = Vector3.zero;

        switch (side) 
        {
            case 0:
                randomLocation = new Vector3(randomCoordiante, -2, outerSpawnSize / 2 - randomOffset);
                break;
            case 1:
                randomLocation = new Vector3(randomCoordiante, -2, -outerSpawnSize / 2 + randomOffset);
                break;
            case 2:
                randomLocation = new Vector3(-outerSpawnSize / 2 + randomOffset, -2, randomCoordiante);
                break;
            case 3:
                randomLocation = new Vector3(outerSpawnSize / 2 - randomOffset, -2, randomCoordiante);
                break;
            default:
                throw new System.Exception("If you see this Error, a cosmic ray must've flipped a bit");
        }

        return randomLocation;
    }

    private int getEnemyCost(EnemyType type)
    {
        switch(type) 
        {
            case EnemyType.IMP:
                return 1;
            default: 
                return 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(innerSpawnSize, 0, innerSpawnSize));
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(outerSpawnSize, 0, outerSpawnSize));
    }
}
