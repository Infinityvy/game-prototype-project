using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildModeState : IPlayerState
{
    // public:
    // private:
    private Transform raftTilePrefab;
    private float tileSize = 1.6f;

    public void initialize()
    {
        raftTilePrefab = Resources.Load<Transform>("RaftTile");
    }

    public void placeTile(int x, int y)
    {
        GameObject.Instantiate(raftTilePrefab, new Vector3(x * tileSize, -0.25f, y * tileSize), Quaternion.identity);
    }
}
