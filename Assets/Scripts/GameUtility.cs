using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtility
{
    public static Vector2Int toTilePosition(this Vector3 position)
    {
        return new Vector2Int(position.x.toTileCoordinate(), position.z.toTileCoordinate());    
    }

    public static int toTileCoordinate(this float coordinate)
    {
        return Mathf.RoundToInt(coordinate / PlayerBuildModeState.tileSize);
    }

    public static bool isMouseOnScreen()
    { 
        return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y); 
    }
}
