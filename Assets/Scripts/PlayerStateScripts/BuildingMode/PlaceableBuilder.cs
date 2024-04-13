using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableBuilder
{
    // private:
    private PlayerBuildModeState buildModeState;

    private static Dictionary<int, Dictionary<int, IPlaceable>> placeables;
    private Transform parent;
    private Dictionary<PlaceableType, Transform> placeablePrefabs;

    public void initialize(PlayerBuildModeState buildModeState)
    {
        this.buildModeState = buildModeState;

        placeables ??= new Dictionary<int, Dictionary<int, IPlaceable>>();

        placeablePrefabs = new Dictionary<PlaceableType, Transform>();
        placeablePrefabs.Add(PlaceableType.CROSSBOW, Resources.Load<Transform>("crossbow-turret"));
    }

    public void placePlaceable(PlaceableType type, Vector2Int pos) { placePlaceable(type, pos.x, pos.y); }
    public void placePlaceable(PlaceableType type, int x, int z)
    {
        if (!placeables.ContainsKey(x)) placeables.Add(x, new Dictionary<int, IPlaceable>());
        placeables[x].Add(z, GameObject.Instantiate(placeablePrefabs[type], new Vector3(x * TileBuilder.tileSize, 0.29f, z * TileBuilder.tileSize), Quaternion.identity, parent).GetComponent<IPlaceable>());
        buildModeState.notifyWorldChange();
    }

    public void removePlaceable(Vector2Int pos) { removePlaceable(pos.x, pos.y); }
    public void removePlaceable(int x, int z)
    {
        GameObject.Destroy(placeables[x][z].transform().gameObject);
        placeables[x].Remove(z);
        if (placeables[x].Count == 0) placeables.Remove(x);
        buildModeState.notifyWorldChange();
    }



    public static IPlaceable getPlaceable(Vector2Int pos) { return getPlaceable(pos.x, pos.y); }
    public static IPlaceable getPlaceable(int x, int z)
    {
        if (!placeables.ContainsKey(x)) return null;
        else if (!placeables[x].ContainsKey(z)) return null;
        else return placeables[x][z];
    }
}
