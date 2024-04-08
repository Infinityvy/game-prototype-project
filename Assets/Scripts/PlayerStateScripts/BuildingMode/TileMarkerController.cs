using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TileMarkerController : MonoBehaviour
{
    // public:
    public Transform tileMarkerPrefab;
    public Texture2D texMarkerOnEmpty;
    public Texture2D texMarkerOnTile;
    public Texture2D texMarkerOnPlaceable;

    // private:
    private Transform[] tileMarkers;

    private Transform player;
    private Vector2Int previousPlayerTilePosition;

    private bool tilesChanged = true;

    private Transform highlightedMarker;
    private Color defaultColor = new Color(0.8f, 0.8f, 0.8f, 0f);
    private Color highlightColor = new Color(3, 3, 3, 1);

    private enum MarkerType
    {
        OnEmpty,
        OnTile,
        OnPlaceable
    }

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

                    Vector2Int tilePosition = new Vector2Int(playerTilePosition.x + x, playerTilePosition.y + z);
                    Transform tileMarker = tileMarkers[tileMarkerIndex];
                    tileMarker.gameObject.SetActive(true);

                    if (PlayerBuildModeState.getPlaceable(tilePosition) != null)
                    {
                        setMarkerType(MarkerType.OnPlaceable, tileMarker);
                    }
                    else if (PlayerBuildModeState.getTile(tilePosition) != null)
                    {
                        setMarkerType(MarkerType.OnTile, tileMarker);
                    }
                    else
                    {
                        if(PlayerBuildModeState.hasAdjacentTile(tilePosition))
                            setMarkerType(MarkerType.OnEmpty, tileMarker);
                        else
                            tileMarker.gameObject.SetActive(false);
                    }

                    tileMarker.position = new Vector3(tilePosition.x * PlayerBuildModeState.tileSize, 0,
                                                                        tilePosition.y * PlayerBuildModeState.tileSize);
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

    private void setMarkerTexture(Transform marker, Texture2D texture)
    {
        marker.GetComponentInChildren<MeshRenderer>().material.SetTexture("_Texture2D", texture);
    }

    private void setMarkerColor(Transform marker, Color color)
    {
        marker.GetComponentInChildren<MeshRenderer>().material.SetColor("_BaseColor", color);
    }

    private void setMarkerType(MarkerType type, Transform marker)
    {
        switch (type) 
        {
            case MarkerType.OnEmpty:
                setMarkerTexture(marker, texMarkerOnEmpty);
                marker.GetComponent<BoxCollider>().center = new Vector3(0f, -0.5f, 0f);
                marker.GetComponent<BoxCollider>().size = new Vector3(1.6f, 0.1f, 1.6f);
                marker.GetChild(0).localPosition = Vector3.zero;
                break;
            case MarkerType.OnTile:
                setMarkerTexture(marker, texMarkerOnTile);
                marker.GetComponent<BoxCollider>().center = new Vector3(0f, -0.13f, 0f);
                marker.GetComponent<BoxCollider>().size = new Vector3(1.6f, 0.9f, 1.6f);
                marker.GetChild(0).localPosition = new Vector3(0, 0.5f, 0);
                break;
            case MarkerType.OnPlaceable:
                setMarkerTexture(marker, texMarkerOnPlaceable);
                marker.GetComponent<BoxCollider>().center = new Vector3(0f, -0.13f, 0f);
                marker.GetComponent<BoxCollider>().size = new Vector3(1.6f, 0.9f, 1.6f);
                marker.GetChild(0).localPosition = new Vector3(0, 0.5f, 0);
                break;
            default:
                break;
        }
    }
}
