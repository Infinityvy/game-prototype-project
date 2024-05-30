using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBuildModeState : IPlayerState
{
    // public:
    public static ResourceInventory resourceInventory { get; private set; }

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

        resourceInventory = new ResourceInventory();
        resourceInventory.initialize();
    }

    public void finalize()
    {
        GameObject.Destroy(tileMarkerController.gameObject);
    }

    public void update()
    {
        if (Session.instance.isPaused) return;
        if(PlayerEntity.instance.isDead) return;
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
            if(!resourceInventory.subtractResources(TileBuilder.tileCost)) return false;

            tileBuilder.placeTile(pos);
            return true;
        }
        else if (PlaceableBuilder.getPlaceable(pos) == null)
        {
            if(!resourceInventory.subtractResources(PlaceableBuilder.getPlaceableCost(PlaceableType.CROSSBOW))) return false;

            placeableBuilder.placePlaceable(PlaceableType.CROSSBOW, pos);
            return true;
        }

        return false;
    }

    private bool tryRemove(Vector2Int pos) 
    {
        if (PlaceableBuilder.getPlaceable(pos) != null)
        {
            resourceInventory.addResources(PlaceableBuilder.getPlaceableCost(PlaceableBuilder.getPlaceable(pos).type) / 2);
            placeableBuilder.removePlaceable(pos);
            return true;
        }
        else if (TileBuilder.getTile(pos) != null)
        {
            resourceInventory.addResources(TileBuilder.tileCost / 2);
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
