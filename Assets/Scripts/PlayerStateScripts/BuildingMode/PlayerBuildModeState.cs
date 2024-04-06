using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBuildModeState : IPlayerState
{
    // public:
    // private:
    public static Dictionary<int, Dictionary<int, Transform>> raftTiles;
    public static readonly float tileSize = 1.6f;

    private Transform raftParent;
    private Transform raftTilePrefab;

    private Transform tileHighlighter;

    public void initialize()
    {
        raftTiles ??= new Dictionary<int, Dictionary<int, Transform>>();

        raftParent = GameObject.Find("Raft").transform;
        raftTilePrefab = Resources.Load<Transform>("RaftTile");

        tileHighlighter = GameObject.Instantiate(Resources.Load<Transform>("TileHighlighter"), Vector3.zero, Quaternion.identity);
    }

    public void finalize()
    {
        // tileHighlighter.GetComponent<TileHighlighter>().destroy();
        GameObject.Destroy(tileHighlighter.gameObject);
    }

    public void update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Highlight")))
            {
                Vector2Int tilePos = hit.point.toTilePosition();

                if(!getTile(tilePos.x, tilePos.y))
                {
                    placeTile(tilePos.x, tilePos.y);
                }
            }
        }
    }

    public void placeTile(int x, int z)
    {
        if(!raftTiles.ContainsKey(x)) raftTiles.Add(x, new Dictionary<int, Transform>());
        raftTiles[x].Add(z, GameObject.Instantiate(raftTilePrefab, new Vector3(x * tileSize, -0.25f, z * tileSize), Quaternion.identity, raftParent));
    }

    private void removeTile(int x, int z)
    {
        GameObject.Destroy(raftTiles[x][z].gameObject);
        raftTiles[x].Remove(z);
        if (raftTiles[x].Count == 0 ) raftTiles.Remove(x);
    }

    public static Transform getTile(int x, int z)
    {
        if (!raftTiles.ContainsKey(x)) return null;
        else if (!raftTiles[x].ContainsKey(z)) return null;
        else return raftTiles[x][z];
    }
}
