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

    private TileMarkerController tileMarkerController;

    public void initialize()
    {
        raftTiles ??= new Dictionary<int, Dictionary<int, Transform>>();

        raftParent = GameObject.Find("Raft").transform;
        raftTilePrefab = Resources.Load<Transform>("RaftTile");

        tileMarkerController = GameObject.Instantiate(Resources.Load<Transform>("TileHighlighter"), Vector3.zero, Quaternion.identity).GetComponent<TileMarkerController>();
    }

    public void finalize()
    {
        GameObject.Destroy(tileMarkerController.gameObject);
    }

    public void update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Highlight")))
        {
            tileMarkerController.setHighlightedMarker(hit.transform);
            Vector2Int tilePos = hit.point.toTilePosition();

            if (Input.GetMouseButtonDown(0) && !getTile(tilePos.x, tilePos.y))
            {
                placeTile(tilePos.x, tilePos.y);
            }
        }
        else
        {
            tileMarkerController.setHighlightedMarker(null);
        }
    }

    public void placeTile(int x, int z)
    {
        if(!raftTiles.ContainsKey(x)) raftTiles.Add(x, new Dictionary<int, Transform>());
        raftTiles[x].Add(z, GameObject.Instantiate(raftTilePrefab, new Vector3(x * tileSize, -0.25f, z * tileSize), Quaternion.identity, raftParent));
        tileMarkerController.GetComponent<TileMarkerController>().notifyTileChange();
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
