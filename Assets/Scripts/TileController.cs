using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [Header("Components")]
    public TileSO tileStats = null; //ScriptableObject with stats for tile
    public PlayerManager playerManager = null;

    protected SpriteRenderer spriteRenderer = null;
    [SerializeField] protected List<TileController> sideTiles = new List<TileController>(); //list of neighbours of this tile

    [Header("Stats")]
    [SerializeField] protected int points; //tile points that will be awarded on destroy
    [SerializeField] protected Color color; //tile color
    [SerializeField] protected TileSO.EType type; //tile type
    public bool isSwapped = false; //variable needed for not stoping instantiate at spawning two objects at once

    [Header("Events")]
    [SerializeField] public IntGameEvent addPointsEvent;

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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.collider.CompareTag("Ground"))
    //        if (CheckTileNeighbours(this, new List<TileController>()) > 2) DestroyTile(this); //if true destroy tiles
    //}

    //When tile is clicked
    private void OnMouseDown()
    {
        //check if its type is normal
        if (type == TileSO.EType.Normal)
        {
            //Check if there are any elements in list
            if (playerManager.selectedTiles.Count > 0)
            {
                if (playerManager.selectedTiles[0] == this)
                {
                    DeselectTile();
                    return;
                }

                //check for neighbours, two tiles need to be the same color
                if (playerManager.selectedTiles[0].CheckTileNeighbours(this, new List<TileController>()) > 2)
                    StartCoroutine(SwapTilesPositions()); //change positions
                else
                {
                    //Check if clicked tile is neighbour of first one
                    if (sideTiles.Contains(playerManager.selectedTiles[0]))
                        PingPongTween(playerManager.selectedTiles[0].spriteRenderer.gameObject, spriteRenderer.gameObject);
                    DeselectTile();
                }
            }
            else SelectTile();
        }
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
            SwapTween(firstTile.gameObject, gameObject);
            isSwapped = true;
            //destroy firstTile
            yield return new WaitForSeconds(0.3f);
            
            firstTile.DestroyTile(new List<TileController>());
        }
        isSwapped = false;
        //Clear selected tile array
        playerManager.selectedTiles.Clear();
    }

    //Check neigbhours of this tile
    //sideCheck, which is used to check its neighbours
    //checkedTiles all checked tiles
    //return int, count of found tiles in chain
    int CheckTileNeighbours(TileController sideCheck, List<TileController> checkedTiles)
    {
        //Add this tile to checked list
        checkedTiles.Add(this);

        //go through each side tile
        foreach (TileController sideTile in sideCheck.sideTiles)
        {
            //check if any sideTile has equal color or is a special tile
            if (sideTile.color.Equals(color) || sideTile.type != TileSO.EType.Normal)
                //check if sideTile is already on list
                if (!checkedTiles.Contains(sideTile))
                    sideTile.CheckTileNeighbours(sideTile, checkedTiles); //check other sideTiles 
        }
        //return checkedTiles count
        return checkedTiles.Count;
    }

    //Destroy this tile
    //checkedTiles all checked tiles
    public virtual void DestroyTile(List<TileController> checkedTiles)
    {
        checkedTiles.Add(this); //add this tile to checked list

        //go through each sideTile in copied array
        foreach (TileController sideTile in sideTiles.ToArray())
        {
            //check if any sideGam has euqal color or is a special tile
            if (sideTile.color.Equals(color) || sideTile.type != TileSO.EType.Normal)
            {
                //check if its not previousTile
                if (!checkedTiles.Contains(sideTile)) sideTile.DestroyTile(checkedTiles); //check for other tiles with same color
            }
            sideTile.sideTiles.Remove(this); //remove this tile from sideTiles arrays
        }
        //Raise event to add points
        addPointsEvent.Raise(points);
        //Destroy this gameobject
        if (gameObject) Destroy(gameObject);
        //Clear selected tile array
        playerManager.selectedTiles.Clear();
    }

    //Update tile stats from scriptableObject
    public virtual void UpdateTileStats()
    {
        points = tileStats.GetPoints;
        type = tileStats.GetEType;
        color = tileStats.GetColor;

        spriteRenderer.sprite = tileStats.GetSprite;

    }

    #region Tweens

    //Swap firstGO gameobject with another
    //firsGO gameobject that will be swapped with secondGO gameobject
    void SwapTween(GameObject firsGO, GameObject secondGO)
    {
        Vector3 firstPosition = firsGO.transform.position;
        Vector3 secondPosition = secondGO.transform.position;

        LeanTween.move(firsGO, secondPosition, 0.2f);
        LeanTween.move(secondGO, firstPosition, 0.2f);
    }

    //PingPong two objects between their positions
    //firsGO gameobject that will be swapped with secondGO gameobject
    void PingPongTween(GameObject firsGO, GameObject secondGO)
    {
        //get their positions
        Vector3 firstPosition = firsGO.transform.position;
        Vector3 secondPosition = secondGO.transform.position;

        LeanTween.move(firsGO, secondPosition, 0.2f).setLoopPingPong(1);
        LeanTween.move(secondGO, firstPosition, 0.2f).setLoopPingPong(1);
    }

    //Remove this tile as selected
    void DeselectTile()
    {
        //Change back sprite to normal size
        LeanTween.scale(playerManager.selectedTiles[0].spriteRenderer.gameObject, new Vector3(0.168f, 0.168f), 0.2f);
        //remove selected tile
        playerManager.selectedTiles.RemoveAt(0);
    }

    //Select this tile
    void SelectTile()
    {
        //add clicked tile to selectedTiles array
        playerManager.selectedTiles.Add(this);
        //Change tile size to bigger
        LeanTween.scale(spriteRenderer.gameObject, new Vector3(0.22f, 0.22f), 0.2f);
    }

    #endregion
}
