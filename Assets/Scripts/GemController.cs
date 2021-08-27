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
        //Get spriteRenderer on Awake
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check for collision with Gem
        if (collision.CompareTag("Gem"))
        {
            //get compontent GemController from collision
            GemController otherGem = collision.GetComponent<GemController>();
            //check if array already has this gem, if not add it
            if (!sideGems.Contains(otherGem)) sideGems.Add(otherGem);
            //check if any neighbour has more than 3 (2+self) same color gems near eachother
           // if (CheckGemNeighbours(this, this) >= 2) DestroyGem(this); //if true destroy gems
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //get compontent GemController from collision
        GemController leftGem = collision.GetComponent<GemController>();
        //Check if there is component
        if (leftGem)    
            //check if gem that left was in array
            if (sideGems.Contains(leftGem)) sideGems.Remove(leftGem); //if true remove that gem
    }

    //When Gem is clicked
    private void OnMouseDown()
    {
        //set initial position to current position
        initialPosition = transform.position;

        //check if its type is normal
        if (type == GemSO.EType.Normal)
        {
            //Check if there are any elements in list
            if (playerManager.selectedGems.Count > 0)
            {
                //check for neigbours, two gems need to be the same color
                if (CheckGemNeighbours(playerManager.selectedGems[0], this) >= 2) StartCoroutine(SwapGemsPositions()); //change positions
                else
                {
                    //Change back sprite to normal size
                    LeanTween.scale(playerManager.selectedGems[0].spriteRenderer.gameObject, new Vector3(0.168f, 0.168f), 0.2f);
                    //Do tween
                    SwapTween(playerManager.selectedGems[0]);
                    ReverseSwapTween(playerManager.selectedGems[0]);
                    //remove selected gem
                    playerManager.selectedGems.RemoveAt(0);
                }
            }
            else
            {
                //add clicked gem to selectedGems array
                playerManager.selectedGems.Add(this);
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

    //Swap positions of two selected gems
    IEnumerator SwapGemsPositions()
    {
        //First clicked gemController
        GemController firstGem = playerManager.selectedGems[0];

        //Check if selected gems are close to eachother
        if (sideGems.Contains(firstGem))
        {
            ////Swap their positions
            SwapTween(firstGem);

            //destroy firstGem with previous gem as this one
            yield return new WaitForSeconds(0.3f);
            firstGem.DestroyGem(this);
        }

        //Clear selected gems array
        playerManager.selectedGems.Clear();
    }

    //Check neigbhours of this gem
    //gemColorToCheck, color of gem which will be compared
    //previousGem previous checkedGem
    //return counter, number of same color gems in same chain
    int CheckGemNeighbours(GemController gemColorToCheck, GemController previosGem)
    {
        //set counter to zero
        int counter = 0;
        //go through each side gem
        foreach (GemController sideGem in sideGems)
        {
            //check if any sideGem has equal color or is a special gem
            if (sideGem.color.Equals(gemColorToCheck.color) || sideGem.type != GemSO.EType.Normal)
            {
                //check if its not previousGem
                if (sideGem != previosGem)
                {
                    //add +1 to counter
                    counter++;
                    //add other counters
                    counter += sideGem.CheckGemNeighbours(this, this);
                }
            }
        }
        //return counter
        return counter;
    }

    //Destroy this gem
    void DestroyGem(GemController previousGem)
    {
        //go through each sideGem in copied array
        foreach (GemController sideGem in sideGems.ToArray())
        {
            //check if any sideGam has euqal color or is a special gem
            if (sideGem.color.Equals(color) || sideGem.type != GemSO.EType.Normal)
            {
                //check if its not previousGem
                if (sideGem != previousGem) sideGem.DestroyGem(this); //check for other gems with same color
            }
            sideGem.sideGems.Remove(this); //remove this gem from sideGems arrays
        }
        //Destroy this gameobject
        if(gameObject) Destroy(gameObject);
        //Clear selected gems array
        playerManager.selectedGems.Clear();
    }

    //Update gem stats from scriptableObject
    public void UpdateGemStats()
    {
        points = gemStats.GetPoints;
        type = gemStats.GetEType;
        color = gemStats.GetColor;

        spriteRenderer.sprite = gemStats.GetSprite;
    }

    #region Tweens

    void SwapTween(GemController gemToSwapWith)
    {
        LeanTween.move(gameObject, gemToSwapWith.initialPosition, 0.2f);
        LeanTween.move(gemToSwapWith.gameObject, initialPosition, 0.2f);
    }

    void ReverseSwapTween(GemController gemToSwapWith)
    {
        LeanTween.move(gemToSwapWith.gameObject, gemToSwapWith.initialPosition, 0.2f).setDelay(0.3f);
        LeanTween.move(gameObject, initialPosition, 0.2f).setDelay(0.3f);
    }

    #endregion
}
