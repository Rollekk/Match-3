using System.Collections.Generic;
using UnityEngine;

namespace TileControllers
{
    [CreateAssetMenu(fileName = "BombTile", menuName = "ScriptableObjects/BombTile")]
    public class BombTile : TileSO
    {
        //Destroy tile
        //checkedTiles list with all already checked tiles
        //returns number of destroyed tiles
        public override int DestroyTile(TileController tileToDestroy)
        {
            //go through each sideTile
            foreach(var sideTile in tileToDestroy.sideTiles.ToArray())
            {
                //check if array is empty or it already contains sideTile
                if (checkedTiles.Count != 0 && checkedTiles.Contains(sideTile)) continue;
                //add sideTile to array of already checkedTiles
                checkedTiles.Add(sideTile);

                //Raise event to add points
                sideTile.tileSO.addPointsEvent.Raise(tilePoints);

                //destroy gameobject of sideTile
                if (sideTile.gameObject) Destroy(sideTile.gameObject);
            }
            //Destroy this gameobject
            if (tileToDestroy.gameObject) Destroy(tileToDestroy.gameObject);
            //Clear selected tile array
            tileToDestroy.playerManager.selectedTiles.Clear();

            return checkedTiles.Count;
        }
    }
}
