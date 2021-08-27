using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawnerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject gemGO; //Gem gameobject to spawn
    [SerializeField] GameObject gemParent; //Gem parent
    public PlayerManager playerManager; 

    [Header("Gem")]
    [SerializeField] GemSO[] normalGemsArray = null; //Array of scriptableobjects with normal Gem
    [SerializeField] GemSO[] specialGemsArray = null; //Array of scriptableobjects with special Gems
    public float gemSpacing = 1.5f; //Spacing between gems

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 7; i++) CreateFirstGemRow(i);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem")) CreateNewGem(collision.transform.position);
    }

    //Create first row of gem
    //i is number of created gems needed for spacing
    void CreateFirstGemRow(int i)
    {
        //create new gem with correct offset and get its controller
        GemController newGem = Instantiate(gemGO, transform.position + new Vector3(i * gemSpacing, 0.0f, 0.0f), gemGO.transform.rotation).GetComponent<GemController>();

        //set newGem transform to new parent
        newGem.transform.parent = gemParent.transform;

        //get random GemSO from array of ScriptableObjects
        newGem.gemStats = GetRandomStats();

        //set newGem manager to spawners manager
        newGem.playerManager = this.playerManager;
        //update gem stats
        newGem.UpdateGemStats();
    }

    //Create one new gem
    //removedGemPosition position of removed gem in game
    void CreateNewGem(Vector3 removedGemPosition)
    {
        //create new gem with given position and get its controller
        GemController newGem = Instantiate(gemGO, new Vector3(removedGemPosition.x, transform.position.y, removedGemPosition.z), gemGO.transform.rotation).GetComponentInChildren<GemController>();

        //set newGem transform to new parent
        newGem.transform.parent = gemParent.transform;
        //get random GemSO from array of ScriptableObjects
        newGem.gemStats = GetRandomStats();
        //set newGem manager to spawners manager
        newGem.playerManager = this.playerManager;
        //update gem stats
        newGem.UpdateGemStats();
    }

    //Get random GemSO from array of ScriptableObjects
    //returns random GemSO
    GemSO GetRandomStats()
    {
        //random probability of getting special gem
        float randomProb = Random.Range(0f, 100f);

        //check probability, return normal or special gem
        if (randomProb <= 95f) return normalGemsArray[(int)Random.Range(0, normalGemsArray.Length)];
        else return specialGemsArray[(int)Random.Range(0, specialGemsArray.Length)];
    }
}
