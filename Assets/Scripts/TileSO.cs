using System.Collections;
using System.Collections.Generic;
using TileControllers;
using UnityEngine;

[CreateAssetMenu(fileName = "GemSO", menuName = "ScriptableObjects/GemSO")]
public class TileSO : ScriptableObject
{
    public enum EType { Normal, Bomb, ColorPicker }

    [Header("Stats")]
    [SerializeField] protected int tilePoints;
    [SerializeField] protected EType tileType;
    [SerializeField] protected Color tileColor;
    [SerializeField] protected Sprite tileSprite;

    public int GetPoints => tilePoints;
    public EType GetEType => tileType;
    public Color GetColor => tileColor;
    public Sprite GetSprite => tileSprite;

    [Header("Events")]
    public IntGameEvent addPointsEvent; //Event called to add points to player
    public IntGameEvent addMovesEvent; //Event called to add points to player

    protected static readonly List<TileController> checkedTiles = new List<TileController>();
    
    //Swap positions of two selected tiles
    //firstTile that will be swapped with second
    public IEnumerator SwapTilesPositions(TileController firstTile, TileController secondTile)
    {
        //Change back sprite to normal size
        LeanTween.scale(secondTile.spriteRenderer.gameObject, new Vector3(0.168f, 0.168f), 0.2f);

        checkedTiles.Clear();
        
        //Check if selected tiles are close to eachother
        if (secondTile.sideTiles.Contains(firstTile))
        {
            ////Swap their positions
            SwapTween(firstTile.gameObject, secondTile.gameObject);
            secondTile.isSwapped = true; //set secondTile to the one being swapped
            //destroy firstTile
            yield return new WaitForSeconds(0.3f);

            //Raise event to add points
            addPointsEvent.Raise(DestroyTile(firstTile) * tilePoints); //Destroy tile and multiply it with points
        }
        //set secondTile to not being swapped anymore
        secondTile.isSwapped = false; 
        
        checkedTiles.Clear();
        
        //Clear selected tile array
        firstTile.playerManager.selectedTiles.Clear();
    }

    //Check neighbours of this tile
    //sideCheck, which is used to check its neighbours
    //checkedTiles all checked tiles
    //return int, count of found tiles in chain
    public int CheckTileNeighbours(TileController sideToCheck, TileController originalTile)
    {
        //Add this tile to checked list
        checkedTiles.Add(originalTile);

        //go through each side tile
        foreach (var sideTile in sideToCheck.sideTiles)
        {
            if (!sideTile.tileSO.GetColor.Equals(originalTile.tileSO.GetColor) &&
                sideTile.tileSO.GetEType == EType.Normal) continue;
            
            if(!checkedTiles.Contains(sideTile))
                sideTile.tileSO.CheckTileNeighbours(sideTile, sideTile); //check other sideTiles 
        }

        //return checkedTiles count
        return checkedTiles.Count;
    }

    public virtual int DestroyTile(TileController tileToDestroy)
    {
        return checkedTiles.Count;
    }

    #region Tweens

    //Swap firstGO gameobject with another
    //firsGO gameobject that will be swapped with secondGO gameobject
    private void SwapTween(GameObject firsGO, GameObject secondGO)
    {
        //Get both gameobjects positions
        var firstPosition = firsGO.transform.position;
        var secondPosition = secondGO.transform.position;

        //Move both gameobjects to different positions
        LeanTween.move(firsGO, secondPosition, 0.2f);
        LeanTween.move(secondGO, firstPosition, 0.2f);

        //Raise event to add playerMoves
        addMovesEvent.Raise(1);
    }

    //PingPong two objects between their positions
    //firsGO gameobject that will be swapped with secondGO gameobject
    public void PingPongTween(GameObject firsGO, GameObject secondGO)
    {
        //Get both gameobjects positions
        var firstPosition = firsGO.transform.position;
        var secondPosition = secondGO.transform.position;

        //Move both gameobjects to different positions
        LeanTween.move(firsGO, secondPosition, 0.2f).setLoopPingPong(1);
        LeanTween.move(secondGO, firstPosition, 0.2f).setLoopPingPong(1);

        //Raise event to add playerMoves
        addMovesEvent.Raise(1);
    }

    #endregion
}
