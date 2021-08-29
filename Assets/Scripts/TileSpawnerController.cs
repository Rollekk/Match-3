using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawnerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject tileGO; //Tile gameobject to spawn
    [SerializeField] GameObject tileParent; //Tile parent
    public PlayerManager playerManager;

    [Header("Tile")]
    [SerializeField] TileSO[] normalTilesArray = null; //Array of scriptableobjects with normal Tile
    [SerializeField] TileSO[] specialTilesArray = null; //Array of scriptableobjects with special Tiles

    [SerializeField] float tileSpacing = 1.5f; //Spacing between tiles
    [SerializeField] int numOfTileColumns = 5; //Number of columns for tiles

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numOfTileColumns; i++) CreateNewTile(transform.position + new Vector3(i * tileSpacing, 0.0f, 0.0f));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
                CreateNewTile(collision.transform.position);
    }

    //Create one new tile
    //removedTilePosition position of removed tile in game
    void CreateNewTile(Vector3 removedTilePosition)
    {
        //create new tile with given position and get its controller
        TileController newTile = Instantiate(tileGO, new Vector3(removedTilePosition.x, transform.position.y, removedTilePosition.z), tileGO.transform.rotation).GetComponentInChildren<TileController>();

        //set newTile transform to new parent
        newTile.transform.parent = tileParent.transform;
        //get random tileSO from array of ScriptableObjects
        newTile.tileStats = GetRandomStats();
        //set newTile manager to spawners manager
        newTile.playerManager = this.playerManager;
        //update tileStats
        newTile.UpdateTileStats();
    }

    //Get random tileSO from array of ScriptableObjects
    //returns random tileSO
    TileSO GetRandomStats()
    {
        //random probability of getting special tile
        float randomProb = Random.Range(0f, 100f);

        //check probability, return normal or special tile
        if (randomProb <= 95f) return normalTilesArray[(int)Random.Range(0, normalTilesArray.Length)];
        else return specialTilesArray[(int)Random.Range(0, specialTilesArray.Length)];
    }
}
