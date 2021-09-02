using System.Collections;
using System.Collections.Generic;
using TileControllers;
using UnityEngine;

public class TopTileRemover : MonoBehaviour
{
    //list of all tiles inside collider
    [SerializeField] private List<TileController> insideTiles = new List<TileController>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Tile")) return;
        var tile = collision.GetComponent<TileController>();

        //Add tile to insideTiles
        insideTiles.Add(tile);
        //Start coroutine for checking collision
        StartCoroutine(CheckCollision(tile));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Tile")) return;
        
        var tile = collision.GetComponent<TileController>();

        //check if tile that left is in List insideTiles
        if (!insideTiles.Contains(tile)) return;
            
        //Stop coroutine for checking collision
        StopCoroutine(CheckCollision(tile));
        //Remove tile that left collision
        insideTiles.Remove(tile);

    }

    //Check if insideTile is colliding
    private IEnumerator CheckCollision(TileController insideTile)
    {
        //Wait one second
        yield return new WaitForSeconds(1.0f);

        //check if insideTile is on list
        if (insideTiles.Contains(insideTile))
            Destroy(insideTile.gameObject); //Destroy insideTile
    }
}
