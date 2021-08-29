using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileController : TileController
{

    #region UnityOverrides

    private void Awake()
    {
        //Get spriteRenderer on Awake
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check for collision with tile
        if (collision.CompareTag("Tile"))
        {
            //get compontent TileController from collision
            TileController otherTile = collision.GetComponent<TileController>();
            //check if array already has this tile, if not add it
            if (!sideTiles.Contains(otherTile)) sideTiles.Add(otherTile);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //get compontent TileController from tile that left trigger
        TileController leftTile = collision.GetComponent<TileController>();
        //Check if there is component
        if (leftTile)
            //check if tile that left was in array
            if (sideTiles.Contains(leftTile)) sideTiles.Remove(leftTile); //if true remove that tile
    }
    #endregion

    public override void UpdateTileStats()
    {
        base.UpdateTileStats();
    }

    public override void DestroyTile(List<TileController> checkedTiles)
    {
        TileController[] allTiles = FindObjectsOfType<TileController>();

        foreach(TileController tile in allTiles)
        {
            if (tile.color == checkedTiles[0].color)
            {
                //Raise event to add points
                tile.addPointsEvent.Raise(points);
                if (tile.gameObject) Destroy(tile.gameObject);
            }
        }

        //Raise event to add points
        addPointsEvent.Raise(points);
        //Destroy this gameobject
        if (gameObject) Destroy(gameObject);
        //Clear selected tile array
        playerManager.selectedTiles.Clear();
    }
}
