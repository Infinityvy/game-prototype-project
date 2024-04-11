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

    public static Dictionary<int, Dictionary<int, IPlaceable>> placeables;

    private Transform raftParent;
    private Transform raftTilePrefab;

    private Dictionary<PlaceableType, Transform> placeablePrefabs;

    private TileMarkerController tileMarkerController;

    public void initialize()
    {
        raftTiles ??= new Dictionary<int, Dictionary<int, Transform>>();
        placeables ??= new Dictionary<int, Dictionary<int, IPlaceable>>();

        raftParent = GameObject.Find("Raft").transform;
        raftTilePrefab = Resources.Load<Transform>("RaftTile");

        placeablePrefabs = new Dictionary<PlaceableType, Transform>();
        placeablePrefabs.Add(PlaceableType.CROSSBOW, Resources.Load<Transform>("crossbow-turret"));

        tileMarkerController = GameObject.Instantiate(Resources.Load<Transform>("TileHighlighter"), Vector3.zero, Quaternion.identity).GetComponent<TileMarkerController>();
    }

    public void finalize()
    {
        GameObject.Destroy(tileMarkerController.gameObject);
    }

    public void update()
    {
        if (!GameUtility.isMouseOnScreen()) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Highlight")))
        {
            tileMarkerController.setHighlightedMarker(hit.transform);
            Vector2Int tilePos = hit.point.toTilePosition();

            if (Input.GetMouseButtonDown(0))
            {
                if(getTile(tilePos.x, tilePos.y) == null)
                    placeTile(tilePos.x, tilePos.y);
                else if(getPlaceable(tilePos.x, tilePos.y) == null)
                    placePlaceable(PlaceableType.CROSSBOW, tilePos.x, tilePos.y);
            }
        }
        else
        {
            tileMarkerController.setHighlightedMarker(null);
        }
    }

    public void placeTile(Vector2Int pos) { placeTile(pos.x, pos.y); }
    public void placeTile(int x, int z)
    {
        if(!raftTiles.ContainsKey(x)) raftTiles.Add(x, new Dictionary<int, Transform>());
        raftTiles[x].Add(z, GameObject.Instantiate(raftTilePrefab, new Vector3(x * tileSize, -0.25f, z * tileSize), Quaternion.identity, raftParent));
        tileMarkerController.GetComponent<TileMarkerController>().notifyTileChange();
    }

    private void removeTile(Vector2Int pos) { removeTile(pos.x, pos.y); }
    private void removeTile(int x, int z)
    {
        GameObject.Destroy(raftTiles[x][z].gameObject);
        raftTiles[x].Remove(z);
        if (raftTiles[x].Count == 0 ) raftTiles.Remove(x);
        tileMarkerController.GetComponent<TileMarkerController>().notifyTileChange();
    }

    private void placePlaceable(PlaceableType type, Vector2Int pos) { placePlaceable(type, pos.x, pos.y); }
    private void placePlaceable(PlaceableType type, int x, int z)
    {
        if (!placeables.ContainsKey(x)) placeables.Add(x, new Dictionary<int, IPlaceable>());
        placeables[x].Add(z, GameObject.Instantiate(placeablePrefabs[type], new Vector3(x * tileSize, 0.29f, z * tileSize), Quaternion.identity, raftParent).GetComponent<IPlaceable>());
        tileMarkerController.GetComponent<TileMarkerController>().notifyTileChange();
    }

    private void removePlaceable(int x, int z)
    {

        tileMarkerController.GetComponent<TileMarkerController>().notifyTileChange();
    }

    public static Transform getTile(Vector2Int pos) { return getTile(pos.x, pos.y); }
    public static Transform getTile(int x, int z)
    {
        if (!raftTiles.ContainsKey(x)) return null;
        else if (!raftTiles[x].ContainsKey(z)) return null;
        else return raftTiles[x][z];
    }

    public static IPlaceable getPlaceable(Vector2Int pos) { return getPlaceable(pos.x, pos.y); }
    public static IPlaceable getPlaceable(int x, int z)
    {
        if (!placeables.ContainsKey(x)) return null;
        else if (!placeables[x].ContainsKey(z)) return null;
        else return placeables[x][z];
    }

    public static bool hasAdjacentTile(Vector2Int pos) { return hasAdjacentTile(pos.x, pos.y); }
    public static bool hasAdjacentTile(int x, int z)
    {
        if (getTile(x + 1, z) != null) return true;
        if (getTile(x - 1, z) != null) return true;
        if (getTile(x, z + 1) != null) return true;
        if (getTile(x, z - 1) != null) return true;

        return false;
    }
}
