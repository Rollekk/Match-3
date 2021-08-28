using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [Header("Components")]
    public TileSO tileStats = null;
    public PlayerManager playerManager;

    SpriteRenderer spriteRenderer = null;
    [SerializeField] List<TileController> sideTiles = new List<TileController>();

    [Header("Stats")]
    [SerializeField] private int points;
    [SerializeField] private Color color;
    [SerializeField] private TileSO.EType type;

    [SerializeField] Vector2 initialPosition;

    #region UnityOverrides

    private void Awake()
    {
        //Get spriteRenderer on Awake
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check for collision with Tile
        if (collision.CompareTag("Tile"))
        {
            //get compontent TileController from collision
            TileController otherTile = collision.GetComponent<TileController>();
            //check if array already has this tile, if not add it
            if (!sideTiles.Contains(otherTile)) sideTiles.Add(otherTile);
            //check if any neighbour has more than 3 (2+self) same color tiles near eachother
           // if (CheckTileNeighbours(this, this) >= 2) DestroyTile(this); //if true destroy Tiles
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //get compontent TileController from collision
        TileController leftTile = collision.GetComponent<TileController>();
        //Check if there is component
        if (leftTile)    
            //check if Tile that left was in array
            if (sideTiles.Contains(leftTile)) sideTiles.Remove(leftTile); //if true remove that Tile
    }

    //When Tile is clicked
    private void OnMouseDown()
    {
        //set initial position to current position
        initialPosition = transform.position;

        //check if its type is normal
        if (type == TileSO.EType.Normal)
        {
            //Check if there are any elements in list
            if (playerManager.selectedTiles.Count > 0)
            {
                //check for neigbours, two Tiles need to be the same color
                if (CheckTileNeighbours(playerManager.selectedTiles[0], this) >= 2) StartCoroutine(SwapTilesPositions()); //change positions
                else
                {
                    //Change back sprite to normal size
                    LeanTween.scale(playerManager.selectedTiles[0].spriteRenderer.gameObject, new Vector3(0.168f, 0.168f), 0.2f);
                    //Do tween
                    SwapTween(playerManager.selectedTiles[0]);
                    ReverseSwapTween(playerManager.selectedTiles[0]);
                    //remove selected tile
                    playerManager.selectedTiles.RemoveAt(0);
                }
            }
            else
            {
                //add clicked tile to selectedTiles array
                playerManager.selectedTiles.Add(this);
                LeanTween.scale(spriteRenderer.gameObject, new Vector3(0.22f, 0.22f), 0.2f);
            }
        }
    }

    //When object is destroyed
    private void OnDestroy()
    {
        //add points to player
        playerManager.AddPointsToPlayer(points);
    }

    #endregion

    //Swap positions of two selected tiles
    IEnumerator SwapTilesPositions()
    {
        //First clicked TileController
        TileController firstTile = playerManager.selectedTiles[0];

        //Check if selected tiles are close to eachother
        if (sideTiles.Contains(firstTile))
        {
            ////Swap their positions
            SwapTween(firstTile);

            //destroy firstTile with previous tile as this one
            yield return new WaitForSeconds(0.3f);
            firstTile.DestroyTile(this);
        }

        //Clear selected tile array
        playerManager.selectedTiles.Clear();
    }

    //Check neigbhours of this tiles
    //TileColorToCheck, color of Tile which will be compared
    //previousTile previous checkedTile
    //return counter, number of same color tiles in same chain
    int CheckTileNeighbours(TileController TileColorToCheck, TileController previosTile)
    {
        //set counter to zero
        int counter = 0;
        //go through each side tile
        foreach (TileController sideTile in sideTiles)
        {
            //check if any sideTile has equal color or is a special tile
            if (sideTile.color.Equals(TileColorToCheck.color) || sideTile.type != TileSO.EType.Normal)
            {
                //check if its not previousTile
                if (sideTile != previosTile)
                {
                    //add +1 to counter
                    counter++;
                    //add other counters
                    counter += sideTile.CheckTileNeighbours(this, this);
                }
            }
        }
        //return counter
        return counter;
    }

    //Destroy this tile
    void DestroyTile(TileController previousTile)
    {
        //go through each sideTile in copied array
        foreach (TileController sideTile in sideTiles.ToArray())
        {
            //check if any sideGam has euqal color or is a special tile
            if (sideTile.color.Equals(color) || sideTile.type != TileSO.EType.Normal)
            {
                //check if its not previousTile
                if (sideTile != previousTile) sideTile.DestroyTile(this); //check for other Tiles with same color
            }
            sideTile.sideTiles.Remove(this); //remove this tile from sideTiles arrays
        }
        //Destroy this gameobject
        if(gameObject) Destroy(gameObject);
        //Clear selected tile array
        playerManager.selectedTiles.Clear();
    }

    //Update Tile stats from scriptableObject
    public void UpdateTileStats()
    {
        points = tileStats.GetPoints;
        type = tileStats.GetEType;
        color = tileStats.GetColor;

        spriteRenderer.sprite = tileStats.GetSprite;
    }

    #region Tweens

    void SwapTween(TileController TileToSwapWith)
    {
        LeanTween.move(gameObject, TileToSwapWith.initialPosition, 0.2f);
        LeanTween.move(TileToSwapWith.gameObject, initialPosition, 0.2f);
    }

    void ReverseSwapTween(TileController TileToSwapWith)
    {
        LeanTween.move(TileToSwapWith.gameObject, TileToSwapWith.initialPosition, 0.2f).setDelay(0.3f);
        LeanTween.move(gameObject, initialPosition, 0.2f).setDelay(0.3f);
    }

    #endregion
}
