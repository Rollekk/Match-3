using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileController : TileController
{
    protected override void OnMouseDown()
    {
        if (playerManager.selectedTiles.Count > 0) playerManager.selectedTiles[0].DeselectTile();
    }

    //Destroy tile
    //checkedTiles list with all already checked tiles
    //returns number of destroyed tiles
    public override int DestroyTile(List<TileController> checkedTiles)
    {
        //Get all tiles in game
        TileController[] allTiles = FindObjectsOfType<TileController>();

        //Go through array of all found tiles
        foreach(TileController tile in allTiles)
        {
            //check if any have the same color as destroyed tile
            if (tile.color == checkedTiles[0].color)
            {
                //Raise event to add points
                tile.addPointsEvent.Raise(points);
                //Destroy tile
                if (tile.gameObject) Destroy(tile.gameObject);
            }
        }
        //Destroy this gameobject
        if (gameObject) Destroy(gameObject);
        //Clear selected tile array
        playerManager.selectedTiles.Clear();

        return checkedTiles.Count;
    }
}
