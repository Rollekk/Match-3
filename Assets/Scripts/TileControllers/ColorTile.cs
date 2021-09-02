using System.Collections.Generic;
using UnityEngine;

namespace TileControllers
{
    [CreateAssetMenu(fileName = "ColorTile", menuName = "ScriptableObjects/ColorTile")]
    public class ColorTile: TileSO
    {
        //Destroy tile
        //checkedTiles list with all already checked tiles
        //returns number of destroyed tiles
        public override int DestroyTile(TileController tileToDestroy)
        {
            //Get all tiles in game
            var allTiles = FindObjectsOfType<TileController>();

            //Go through array of all found tiles
            foreach(var tile in allTiles)
            {
                //check if any have the same color as destroyed tile
                if (tile.tileSO.GetColor != tileToDestroy.tileSO.GetColor) continue;
                //Raise event to add points
                tile.tileSO.addPointsEvent.Raise(tilePoints);
                //Destroy tile
                if (tile.gameObject) Destroy(tile.gameObject);
            }
            //Destroy this gameobject
            if (tileToDestroy.gameObject) Destroy(tileToDestroy.gameObject);
            //Clear selected tile array
            tileToDestroy.playerManager.selectedTiles.Clear();

            return checkedTiles.Count;
        }
    }
}
