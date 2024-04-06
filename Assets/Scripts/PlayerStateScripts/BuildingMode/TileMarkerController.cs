using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TileMarkerController : MonoBehaviour
{
    // public:
    public Transform tileMarkerPrefab;
    public Texture2D texMarkerEmpty;
    public Texture2D texMarkerOccupied;

    // private:
    private Transform[] tileMarkers;

    private Transform player;
    private Vector2Int previousPlayerTilePosition;

    private bool tilesChanged = false;

    private Transform highlightedMarker;
    private Color defaultColor = new Color(0.8f, 0.8f, 0.8f, 0.2f);
    private Color highlightColor = new Color(3, 3, 3, 1);

    void Start()
    {
        tileMarkers = new Transform[8];
        for (int i = 0; i < 8; i++)
        {
            tileMarkers[i] = GameObject.Instantiate(tileMarkerPrefab, this.transform);
            tileMarkers[i].gameObject.SetActive(false);
        }

        player = Session.instance.player;
        previousPlayerTilePosition = Vector2Int.zero;
    }

    void Update()
    {
        if (Session.instance.playerState.GetType() != typeof(PlayerBuildModeState)) return;

        Vector2Int playerTilePosition = player.position.toTilePosition();

        if(playerTilePosition != previousPlayerTilePosition || tilesChanged)
        {
            previousPlayerTilePosition = playerTilePosition;
            tilesChanged = false;
            uint tileMarkerIndex = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int z = -1; z < 2; z++)
                {
                    if (x == 0 && z == 0) continue;

                    float y = 0;
                    if (PlayerBuildModeState.getTile(playerTilePosition.x + x, playerTilePosition.y + z))
                    {                        
                        y = 0.5f;
                        setMarkerTexture(tileMarkers[tileMarkerIndex], texMarkerOccupied);
                    }
                    else
                    {
                        setMarkerTexture(tileMarkers[tileMarkerIndex], texMarkerEmpty);
                    }

                    tileMarkers[tileMarkerIndex].gameObject.SetActive(true);
                    tileMarkers[tileMarkerIndex].position = new Vector3((playerTilePosition.x + x) * PlayerBuildModeState.tileSize, y,
                                                                              (playerTilePosition.y + z) * PlayerBuildModeState.tileSize);
                    tileMarkerIndex++;
                }
            }
        }
    }

    public void notifyTileChange()
    {
        tilesChanged = true;
    }

    public void setHighlightedMarker(Transform marker)
    {
        if (marker == null && highlightedMarker == null) return;
        if (marker == highlightedMarker) return;

        

        if (highlightedMarker != null) setMarkerColor(highlightedMarker, defaultColor);
        if (marker != null) setMarkerColor(marker, highlightColor);
        highlightedMarker = marker;
    }

    private void setMarkerTexture(Transform tile, Texture2D texture)
    {
        tile.GetComponentInChildren<MeshRenderer>().material.SetTexture("_Texture2D", texture);
    }

    private void setMarkerColor(Transform tile, Color color)
    {
        tile.GetComponentInChildren<MeshRenderer>().material.SetColor("_BaseColor", color);
    }
}
