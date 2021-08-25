using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    public List<GemController> selectedGems = new List<GemController>();

    [Header("Stats")]
    [SerializeField] int playerPoints;

    //Add points to player points
    void AddPointsToPlayer(int amountOfPoints)
    {
        playerPoints += amountOfPoints;
    }
}
