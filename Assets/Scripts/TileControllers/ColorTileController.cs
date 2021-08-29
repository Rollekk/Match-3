using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileController : TileController
{
    //DestroyTile override from TileController
    public override int DestroyTile(List<TileController> checkedTiles)
    {
        //Get all tiles in game
        TileController[] allTiles = FindObjectsOfType<TileController>();

        //Go through whole array
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
