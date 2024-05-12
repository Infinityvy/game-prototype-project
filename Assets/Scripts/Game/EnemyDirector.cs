using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    public static EnemyDirector instance {  get; private set; }

    public List<IEnemy> enemies = new List<IEnemy>();

    // spawn variables
    private static readonly float spawnAreaThickness = 4f * TileBuilder.tileSize;
    private static readonly float innerSpawnSize = (TileBuilder.maxSize + 1) * TileBuilder.tileSize;
    private static readonly float outerSpawnSize = innerSpawnSize + 2 * spawnAreaThickness;

    void Start()
    {
        instance = this;

        InvokeRepeating(nameof(spawnImp), 0.0f, 15.0f);

    }

    void Update()
    {
        
    }

    private void spawnImp() { spawnEnemy(EnemyType.IMP); }
    private void spawnEnemy(EnemyType type)
    {
        switch (type) 
        {
            case EnemyType.IMP:
                enemies.Add(Instantiate(ImpEnemy.getPrefab(), findSpawnLocation(), Quaternion.identity).AddComponent<ImpEnemy>());
                break;
            default:
                break;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(innerSpawnSize, 0, innerSpawnSize));
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(outerSpawnSize, 0, outerSpawnSize));
    }
}
