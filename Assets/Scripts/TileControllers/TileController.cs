using System.Collections.Generic;
using UnityEngine;

namespace TileControllers
{
    public class TileController : MonoBehaviour
    {
        [Header("Components")]
        public TileSO tileSO; //ScriptableObject with stats for tile
        public PlayerManager playerManager;
        public SpriteRenderer spriteRenderer;
        public List<TileController> sideTiles = new List<TileController>(); //list of neighbours of this tile

        [Header("Stats")]
        public bool isSwapped; //is cube swapped with another, used for not spawning additional fruit on top

        #region UnityOverrides

        private void Awake()
        {
            //Get spriteRenderer on Awake
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            spriteRenderer.sprite = tileSO.GetSprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Check for collision with tile
            if (!collision.CompareTag("Tile")) return;
            //get component TileController from collision
            var otherTile = collision.GetComponent<TileController>();
            //check if array already has this tile, if not add it
            if (!sideTiles.Contains(otherTile)) sideTiles.Add(otherTile);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //get component TileController from tile that left trigger
            var leftTile = collision.GetComponent<TileController>();
            //Check if there is component
            if (!leftTile) return;
            if (sideTiles.Contains(leftTile)) sideTiles.Remove(leftTile); //if true remove that tile
        }

        //When tile is clicked
        protected virtual void OnMouseDown()
        {
            if (tileSO.GetEType != TileSO.EType.Normal) return;
            
            //Check if there are any elements in list
            if (playerManager.selectedTiles.Count > 0)
            {
                //Check if clicked tile is neighbour of first one
                if (sideTiles.Contains(playerManager.selectedTiles[0]))
                {
                    //check first clicked tile for neighbours with same color
                    if (tileSO.CheckTileNeighbours(this, playerManager.selectedTiles[0]) > 2)
                        StartCoroutine(tileSO.SwapTilesPositions(playerManager.selectedTiles[0], this)); //change positions
                    //check second clicked tile for neighbours with same color
                    else if (tileSO.CheckTileNeighbours(playerManager.selectedTiles[0],this) > 2)
                        StartCoroutine(tileSO.SwapTilesPositions(this, playerManager.selectedTiles[0]));//change positions
                    //Do PingPongTween, with two selected tiles  
                    else tileSO.PingPongTween(playerManager.selectedTiles[0].spriteRenderer.gameObject, spriteRenderer.gameObject);

                    DeselectTile();
                }
                else DeselectTile();
            }
            else SelectTile();
        }

        #endregion

        //Deselect tile that is currently selected
        private void DeselectTile()
        {
            //Change back sprite to normal size
            LeanTween.scale(playerManager.selectedTiles[0].spriteRenderer.gameObject, new Vector3(0.168f, 0.168f), 0.2f);
            //remove selected tile
            playerManager.selectedTiles.RemoveAt(0);
        }

        //Select this tile
        private void SelectTile()
        {
            //add clicked tile to selectedTiles array
            playerManager.selectedTiles.Add(this);
            //Change tile size to bigger
            LeanTween.scale(spriteRenderer.gameObject, new Vector3(0.22f, 0.22f), 0.2f);
        }
    }
}
