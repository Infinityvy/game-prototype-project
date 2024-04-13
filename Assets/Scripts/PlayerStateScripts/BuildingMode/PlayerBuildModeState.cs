using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBuildModeState : IPlayerState
{
    // public:
    // private:
    private TileMarkerController tileMarkerController;

    private TileBuilder tileBuilder;
    private PlaceableBuilder placeableBuilder;

    public void initialize()
    {
        tileMarkerController = GameObject.Instantiate(Resources.Load<Transform>("TileMarkerController"), Vector3.zero, Quaternion.identity).GetComponent<TileMarkerController>();

        tileBuilder = new();
        tileBuilder.initialize(this);

        placeableBuilder = new();
        placeableBuilder.initialize(this);
    }

    public void finalize()
    {
        GameObject.Destroy(tileMarkerController.gameObject);
    }

    public void update()
    {
        if (!GameUtility.isMouseOnScreen()) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("TileMarker")))
        {
            tileMarkerController.setHighlightedMarker(hit.transform);
            Vector2Int tilePos = hit.transform.position.toTilePosition();

            if (Input.GetMouseButtonDown(0))
            {
                tryPlace(tilePos);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                tryRemove(tilePos);
            }
        }
        else
        {
            tileMarkerController.setHighlightedMarker(null);
        }
    }

    public void notifyWorldChange()
    {
        tileMarkerController.GetComponent<TileMarkerController>().notifyWorldChange();
    }

    private bool tryPlace(Vector2Int pos)
    {
        if (TileBuilder.getTile(pos) == null)
        {
            tileBuilder.placeTile(pos);
            return true;
        }
        else if (PlaceableBuilder.getPlaceable(pos) == null)
        {
            placeableBuilder.placePlaceable(PlaceableType.CROSSBOW, pos);
            return true;
        }

        return false;
    }

    private bool tryRemove(Vector2Int pos) 
    {
        if (PlaceableBuilder.getPlaceable(pos) != null)
        {
            placeableBuilder.removePlaceable(pos);
            return true;
        }
        else if (TileBuilder.getTile(pos) != null)
        {
            return tileBuilder.removeTile(pos);
        }

        return false;
    }

    public void createStartPlatform()
    {
        for (int x = -1; x < 2; x++)
        {
            for (int z = -1; z < 2; z++)
            {
                tileBuilder.placeTile(x, z);
            }
        }
    }
}
