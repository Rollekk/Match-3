using System.Collections.Generic;
using UnityEngine;

namespace TileControllers
{
    [CreateAssetMenu(fileName = "NormalTile", menuName = "ScriptableObjects/NormalTile")]
    public class NormalTile : TileSO
    {
        //Destroy this tile
        //checkedTiles list with all already checked tiles
        //returns number of destroyed tiles
        public override int DestroyTile(TileController tileToDestroy)
        {
            checkedTiles.Add(tileToDestroy); //add this tile to checked list

            //go through each sideTile in copied array
            foreach (var sideTile in tileToDestroy.sideTiles.ToArray())
            {
                //check if any sideGam has equal color or is a special tile
                if (sideTile.tileSO.GetColor.Equals(tileToDestroy.tileSO.GetColor) || sideTile.tileSO.GetEType != EType.Normal)
                {
                    //check if its not previousTile
                    if (!checkedTiles.Contains(sideTile)) sideTile.tileSO.DestroyTile(sideTile); //check for other tiles with same color
                }
                sideTile.sideTiles.Remove(tileToDestroy); //remove this tile from sideTiles arrays
            }
            //check if tile was swapped
            if (tileToDestroy.isSwapped) tileToDestroy.isSwapped = false; //set isSwapped to false to spawn new tile
            //Destroy this gameobject
            if (tileToDestroy.gameObject) Destroy(tileToDestroy.gameObject);
            //Clear selected tile array
            tileToDestroy.playerManager.selectedTiles.Clear();

            return checkedTiles.Count;
        }
    }
}
