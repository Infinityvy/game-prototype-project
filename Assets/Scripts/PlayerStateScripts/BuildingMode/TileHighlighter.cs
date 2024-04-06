using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    public Transform tileHighlightPrefab;

    private Transform[] tileHighlights;

    private Transform player;
    private Vector2Int previousPlayerTilePosition;

    void Start()
    {
        tileHighlights = new Transform[8];
        for (int i = 0; i < 8; i++)
        {
            tileHighlights[i] = GameObject.Instantiate(tileHighlightPrefab, this.transform);
            tileHighlights[i].gameObject.SetActive(false);
        }

        player = Session.instance.player;
        previousPlayerTilePosition = Vector2Int.zero;
    }

    void Update()
    {
        if (Session.instance.playerState.GetType() != typeof(PlayerBuildModeState)) return;

        Vector2Int playerTilePosition = player.position.toTilePosition();

        if(playerTilePosition != previousPlayerTilePosition)
        {
            previousPlayerTilePosition = playerTilePosition;
            uint tileHighlightIndex = 0;
            for (int x = -1; x < 2; x++)
            {
                for(int z = -1; z < 2; z++)
                {
                    if (x == 0 && z == 0) continue;

                    float y = 0;
                    if(PlayerBuildModeState.getTile(playerTilePosition.x + x, playerTilePosition.y + z))
                        y = 0.5f;

                    tileHighlights[tileHighlightIndex].gameObject.SetActive(true);
                    tileHighlights[tileHighlightIndex].position = new Vector3((playerTilePosition.x + x) * PlayerBuildModeState.tileSize, y,
                                                                              (playerTilePosition.y + z) * PlayerBuildModeState.tileSize);
                    tileHighlightIndex++;
                }
            }
        }
    }
}
