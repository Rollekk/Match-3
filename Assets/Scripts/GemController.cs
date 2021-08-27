using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [Header("Components")]
    public GemSO gemStats = null;
    public PlayerManager playerManager;

    SpriteRenderer spriteRenderer = null;
    [SerializeField] List<GemController> sideGems = new List<GemController>();

    [Header("Stats")]
    [SerializeField] private int points;
    [SerializeField] private Color color;
    [SerializeField] private GemSO.EType type;

    [SerializeField] Vector2 initialPosition;

    #region UnityOverrides

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem"))
        {
            GemController otherGem = collision.GetComponent<GemController>();
            if (!sideGems.Contains(otherGem)) sideGems.Add(otherGem);
            //if (CheckGemNeighbours(this, this) >= 2) DestroyGem(this);
        }
    }

    private void OnMouseDown()
    {
        initialPosition = transform.position;

        if (type == GemSO.EType.Normal)
        {
            //Check if there are any elements in list
            if (playerManager.selectedGems.Count > 0)
            {
                if (CheckGemNeighbours(playerManager.selectedGems[0], this) >= 2) SwapGemsPositions(); //if it's not change positions with other
                else playerManager.selectedGems.RemoveAt(0); //if it's same element remove this one
            }
            else
            {
                playerManager.selectedGems.Add(this); //if there are not any elements, add this one
            }
        }
    }

    private void OnDestroy()
    {
        playerManager.AddPointsToPlayer(points);
    }

    #endregion

    //Swap positions of two selected gems
    void SwapGemsPositions()
    {
        Debug.Log("SWAP");
        //First clicked gemController
        GemController firstGem = playerManager.selectedGems[0];

        //Check if selected gems are close to eachother
        if (sideGems.Contains(firstGem))
        {
            //if they are swap their positions
            gameObject.transform.position = firstGem.initialPosition;
            firstGem.transform.position = initialPosition;

            firstGem.sideGems.Clear();
            firstGem.sideGems = sideGems;
            firstGem.sideGems.Add(this);
            firstGem.sideGems.Remove(firstGem);

            firstGem.DestroyGem(this);
        }
        playerManager.selectedGems.Clear();
    }

    //Check gemToCheck neighbours colors and types
    //return true if color is the same and type is different than normal
    //else return false
    int CheckGemNeighbours(GemController gemToCheck, GemController previosGem)
    {
        int counter = 0;
        foreach (GemController sideGem in sideGems)
        {
            if (sideGem.color.Equals(gemToCheck.color) || sideGem.type != GemSO.EType.Normal)
            {
                if (sideGem != previosGem)
                {
                    counter++;
                    counter += sideGem.CheckGemNeighbours(this, this);
                }
            }
        }
        return counter;
    }

    //Destroy gemToDestroy and all connected gems with same color
    void DestroyGem(GemController previousGem)
    {
        //Add points to player
        foreach (GemController sideGem in sideGems)
        {
            if (sideGem.color.Equals(color) || sideGem.type != GemSO.EType.Normal)
                if (sideGem != previousGem) sideGem.DestroyGem(this);
        }
        if(gameObject) Destroy(gameObject);
        playerManager.selectedGems.Clear();
    }

    //Update gem stats from scriptableObject
    public void UpdateGemStats()
    {
        points = gemStats.GetPoints;
        type = gemStats.GetEType;
        color = gemStats.GetColor;

        spriteRenderer.color = color;
    }
}
