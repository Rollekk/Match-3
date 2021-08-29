using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    public List<TileController> selectedTiles = new List<TileController>();
    [SerializeField] UIController uiController;

    [Header("Stats")]
    [SerializeField] int playerPoints = 0;
    [SerializeField] int playerMoves = 0;

    //Add points to player points
    public void AddPointsToPlayer(int amountOfPoints)
    {
        playerPoints += amountOfPoints;
        uiController.UpdatePointsText(playerPoints);
    }

    //Add moves to player
    public void AddMovesToPlayer(int amountOfMoves)
    {
        playerMoves += amountOfMoves;
        uiController.UpdateMovesText(playerMoves);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
