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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateGameStats()
    {
        points = gemStats.GetPoints;
        type = gemStats.GetEType;

        color = gemStats.GetColor;
        spriteRenderer.color = gemStats.GetColor;
    }

    private void OnMouseDown()
    {
        //Check if there are any elements in list
        if(playerManager.selectedGems.Count > 0 )
        {
            //Check if first element in list is this one
            if (playerManager.selectedGems[0] != this)
            {
                //if it's not change positions with other...
                //...and clear list
                Debug.Log("Change positions");
                playerManager.selectedGems.Clear();
            }
            else
            {
                //if it's same element remove this one
                Debug.Log("Remove this gem");
                playerManager.selectedGems.RemoveAt(0);
            }
        }
        else
        {
            //if there are not any elements, add this one
            Debug.Log("Add new");
            playerManager.selectedGems.Add(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gem"))
        {
            GemController otherGem = collision.GetComponent<GemController>();
            if (!sideGems.Contains(otherGem)) sideGems.Add(otherGem);
        }
    }
}
