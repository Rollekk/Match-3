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
        }
    }

    private void OnMouseDown()
    {
        initialPosition = transform.position;

        //Check if there are any elements in list
        if (playerManager.selectedGems.Count > 0 )
        {
            //Check if first element in list is this one
            if (playerManager.selectedGems[0] != this) SwapGemsPositions(); //if it's not change positions with other
            else  //if it's same element remove this one
            {
                Debug.Log("Remove this gem");
                playerManager.selectedGems.RemoveAt(0);
            }
        }
        else
        {
            //if there are not any elements, add this one
            Debug.Log("Add new");
            playerManager.selectedGems.Add(this);
            CheckColors();
        }
    }

    #endregion

    //Update gem stats from scriptableObject
    public void UpdateGemStats()
    {
        points = gemStats.GetPoints;
        type = gemStats.GetEType;
        color = gemStats.GetColor;

        spriteRenderer.color = color;
    }

    //Swap positions of two selected gems
    void SwapGemsPositions()
    {
        GemController otherGem = playerManager.selectedGems[0];

        //Check if selected gems are close to eachother
        if (sideGems.Contains(otherGem))
        {
            //if they are swap their positions
            gameObject.transform.position = otherGem.initialPosition;
            otherGem.transform.position = initialPosition;

            //clear list to fill it with new sideGems
            sideGems.Clear();
            //add swapped gem to list
            sideGems.Add(otherGem);

            //clear list to fill it with new sideGems
            otherGem.sideGems.Clear();
            //add swapped gem to list
            otherGem.sideGems.Add(this);
        }
        playerManager.selectedGems.Clear();
    }

    //Check side gem colors
    void CheckColors()
    {
        //For each gem in side gems...
        foreach (GemController sideGem in sideGems)
        {
            ////...check if their color equals this gem
            if (sideGem.color.Equals(this.color))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        playerManager.AddPointsToPlayer(points);

        foreach (GemController sideGem in sideGems)
        {
            if (sideGem.color.Equals(this.color))
                Destroy(sideGem.gameObject);
            sideGem.sideGems.Remove(this);
        }
        playerManager.selectedGems.Clear();
    }
}
