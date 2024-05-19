using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileBuilder
{
    // public:
    public static readonly float tileSize = 1.6f;
    public static readonly uint maxSize = 15;
    public static readonly ResourceBlock tileCost = new ResourceBlock(2, 0);

    // private:
    private PlayerBuildModeState buildModeState;

    private static Dictionary<int, Dictionary<int, Transform>> tiles;
    private Transform parent;
    private Transform tilePrefab;

    public void initialize(PlayerBuildModeState buildModeState)
    {
        this.buildModeState = buildModeState;

        tiles ??= new Dictionary<int, Dictionary<int, Transform>>();

        parent = GameObject.Find("Raft").transform;
        tilePrefab = Resources.Load<Transform>("RaftTile");
    }

    public void placeTile(Vector2Int pos) { placeTile(pos.x, pos.y); }
    public void placeTile(int x, int y)
    {
        if (!tiles.ContainsKey(x)) tiles.Add(x, new Dictionary<int, Transform>());
        tiles[x].Add(y, GameObject.Instantiate(tilePrefab, new Vector3(x * tileSize, -0.25f, y * tileSize), Quaternion.identity, parent));
        buildModeState.notifyWorldChange();
    }

    public bool removeTile(Vector2Int pos) { return removeTile(pos.x, pos.y); }
    public bool removeTile(int x, int y)
    {
        GameObject.Destroy(tiles[x][y].gameObject);
        tiles[x].Remove(y);
        if (tiles[x].Count == 0) tiles.Remove(x);

        if(!areNeighborsConnected(x, y))
        {
            placeTile(x, y);
            return false;
        }

        buildModeState.notifyWorldChange();
        return true;
    }

    public static Transform getTile(Vector2Int pos) { return getTile(pos.x, pos.y); }
    public static Transform getTile(int x, int y)
    {
        if (!tiles.ContainsKey(x)) return null;
        else if (!tiles[x].ContainsKey(y)) return null;
        else return tiles[x][y];
    }

    public static Transform[] getAdjacentTiles(Vector2Int pos) {  return getAdjacentTiles(pos.x, pos.y); }
    public static Transform[] getAdjacentTiles(int x, int y)
    {
        List<Transform> neighbors = new List<Transform>();

        if (getTile(x + 1, y) != null) neighbors.Add(getTile(x + 1, y));
        if (getTile(x - 1, y) != null) neighbors.Add(getTile(x - 1, y));
        if (getTile(x, y + 1) != null) neighbors.Add(getTile(x, y + 1));
        if (getTile(x, y - 1) != null) neighbors.Add(getTile(x, y - 1));

        return neighbors.ToArray();
    }

    public static bool hasAdjacentTile(Vector2Int pos) { return hasAdjacentTile(pos.x, pos.y); }
    public static bool hasAdjacentTile(int x, int y)
    {
        if(getAdjacentTiles(x, y).Length > 0) return true;

        return false;
    }

    private bool areNeighborsConnected(Vector2Int pos) { return areNeighborsConnected(pos.x, pos.y); }
    private bool areNeighborsConnected(int x, int y)
    {
        List<Transform> neighbors = getAdjacentTiles(x, y).ToList();

        if(neighbors.Count == 0) return false;
        if(neighbors.Count == 1) return true;

        Vector2Int startPos = neighbors[0].position.toTilePosition();
        neighbors.RemoveAt(0);

        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startPos);

        while(queue.Count > 0) 
        {
            Vector2Int current = queue.Dequeue();
            visited.Add(current);

            foreach(Transform neighbor in getAdjacentTiles(current))
            {
                Vector2Int neighborPos = neighbor.position.toTilePosition();
                if(!visited.Contains(neighborPos))
                {
                    // only removes if it is contained
                    neighbors.Remove(neighbor);
                    if(neighbors.Count == 0) return true;

                    queue.Enqueue(neighborPos);
                }
            }
        }

        // sanity check; if it get to this point this check should always be false, then again if its true it should be fine anyways
        return visited.Count == tiles.Count;
    }

    public static bool isInBounds(Vector2Int pos) { return isInBounds(pos.x, pos.y); }
    public static bool isInBounds(int x, int y)
    {
        if (x < -maxSize/2 || y < -maxSize/2 || x > maxSize/2 || y > maxSize/2)
            return false;

        return true;
    }
}
