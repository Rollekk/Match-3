using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnerController : MonoBehaviour
{
    [Header("Components")]
    public GameObject[] tileGO; //Tile gameobject to spawn
    public PlayerManager playerManager;
    [SerializeField] GameObject randomTileGO; //random tile that will be spawned
    [SerializeField] GameObject tileSpawnParent; //Parent for tiles to spawn in 

    [Header("Tile")]
    [SerializeField] TileSO[] normalTilesArray = null; //Array of scriptableobjects with normal Tile
    [SerializeField] float tileSpacing = 1.5f; //Spacing between tiles
    [SerializeField] int numOfTileColumns = 5; //Number of columns for tiles

    // Start is called before the first frame update
    void Start()
    {
        //Create numOfTileColumns of new tiles
        for (int i = 0; i < numOfTileColumns; i++) CreateNewTile(transform.position + new Vector3(i * tileSpacing, 0.0f, 0.0f));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TileController tile = collision.GetComponentInParent<TileController>(); //Get TileController from colliding object

        if (collision.CompareTag("Ground"))
            //check if there is tile and if its being swapped
            if(tile && !tile.isSwapped) CreateNewTile(collision.transform.position);
    }

    //Create new tile
    //removedTilePosition position of removed tile in game
    void CreateNewTile(Vector3 removedTilePosition)
    {
        randomTileGO = GetRandomTile();
        //create new tile with given position and get its controller
        TileController newTile = Instantiate(randomTileGO, new Vector3(removedTilePosition.x, transform.position.y, removedTilePosition.z), randomTileGO.transform.rotation).GetComponentInChildren<TileController>();

        //set newTile transform to new parent
        newTile.transform.parent = tileSpawnParent.transform;
        //set newTile manager to spawners manager
        newTile.playerManager = playerManager;
        //update tileStats
        newTile.UpdateTileStats();
    }

    //Get random tileSO from array of ScriptableObjects
    //returns random tileSO
    GameObject GetRandomTile()
    {
        //random probability of getting special tile
        float randomProb = Random.Range(0f, 100f);

        //check probability, return normal or special tile
        if (randomProb <= 95f)
        {
            tileGO[0].GetComponent<TileController>().tileStats = normalTilesArray[(int)Random.Range(0, normalTilesArray.Length)];
            return tileGO[0];
        }
        else return tileGO[(int)Random.Range(1, tileGO.Length)];
    }
}
