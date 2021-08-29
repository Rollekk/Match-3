using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopTileRemover : MonoBehaviour
{
    List<TileController> insideTiles = new List<TileController>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tile"))
        {
            TileController tile = collision.GetComponent<TileController>();

            insideTiles.Add(tile);
            StartCoroutine(CheckCollision(tile));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Tile"))
        {
            TileController tile = collision.GetComponent<TileController>();

            if (insideTiles.Contains(tile))
            {
                StopCoroutine(CheckCollision(tile));
                insideTiles.Remove(tile);
            }
                
        }

    }

    IEnumerator CheckCollision(TileController insideTile)
    {
        yield return new WaitForSeconds(1.0f);

        if (insideTiles.Contains(insideTile)) Destroy(insideTile.gameObject);
    }
}
