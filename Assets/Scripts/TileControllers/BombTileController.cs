using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTileController : TileController
{
    //DestroyTile override from TileController
    public override int DestroyTile(List<TileController> checkedTiles)
    {
        //go through each sideTile
        foreach(TileController sideTile in sideTiles.ToArray())
        {
            //check if array is empty or it already contains sideTile
            if(checkedTiles.Count == 0 || !checkedTiles.Contains(sideTile))
            {
                //add sideTile to array of already checkedTiles
                checkedTiles.Add(sideTile);
                //destroy gameobject of sideTile
                if (sideTile.gameObject) Destroy(sideTile.gameObject);

                //Raise event to add points
                sideTile.addPointsEvent.Raise(points);
            }
        }
        //Destroy this gameobject
        if (gameObject) Destroy(gameObject);
        //Clear selected tile array
        playerManager.selectedTiles.Clear();

        return checkedTiles.Count;
    }
}
