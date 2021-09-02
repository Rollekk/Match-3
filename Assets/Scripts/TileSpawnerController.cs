using TileControllers;
using UnityEngine;

public class TileSpawnerController : MonoBehaviour
{
    [Header("Components")]
    public GameObject tileGO; //Tile gameobject to spawn
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameObject tileSpawnParent; //Parent for tiles to spawn in 

    [Header("Tile")] [SerializeField] private TileSO[] normalTilesArray; //Array of scriptableobjects with normal Tile
    [SerializeField] private TileSO[] specialTilesArray; //Array of scriptableobjects with normal Tile
    [SerializeField] private float tileSpacing = 1.5f; //Spacing between tiles
    [SerializeField] private int numOfTileColumns = 5; //Number of columns for tiles

    // Start is called before the first frame update
    private void Start()
    {
        //Create numOfTileColumns of new tiles
        for (var i = 0; i < numOfTileColumns; i++) CreateNewTile(transform.position + new Vector3(i * tileSpacing, 0.0f, 0.0f));

        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var tile = collision.GetComponentInParent<TileController>(); //Get TileController from colliding object

        if (!collision.CompareTag("Ground")) return;
        
        if(tile && !tile.isSwapped) CreateNewTile(collision.transform.position);
    }

    //Create new tile
    //removedTilePosition position of removed tile in game
    private void CreateNewTile(Vector3 removedTilePosition)
    {
        //create new tile with given position and get its controller
        var newTile = Instantiate(tileGO, new Vector3(removedTilePosition.x, transform.position.y, removedTilePosition.z), tileGO.transform.rotation).GetComponentInChildren<TileController>();

        //random probability of getting special tile
        var randomProb = Random.Range(0, 100);

        //check probability, return normal or special tile
        newTile.GetComponent<TileController>().tileSO = randomProb <= 95 ? normalTilesArray[Random.Range(0, normalTilesArray.Length)] : specialTilesArray[(int)Random.Range(0, specialTilesArray.Length)];

        //set newTile transform to new parent
        newTile.transform.parent = tileSpawnParent.transform;
        //set newTile manager to spawners manager
        newTile.playerManager = playerManager;
    }

}
