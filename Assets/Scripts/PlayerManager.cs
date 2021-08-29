using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    public List<TileController> selectedTiles = new List<TileController>();
    [SerializeField] UIController uiController;

    [Header("Stats")]
    [SerializeField] int playerPoints;

    //Add points to player points
    public void AddPointsToPlayer(int amountOfPoints)
    {
        playerPoints += amountOfPoints;
        uiController.UpdatePointsText(playerPoints);
    }
}
