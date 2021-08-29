using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Components")]
    Text playerPointsText;
    [SerializeField] Text playerMovesText;

    private void Awake()
    {
        playerPointsText = GetComponentInChildren<Text>();
    }

    public void UpdatePointsText(int newPointsValue)
    {
        if(playerPointsText) playerPointsText.text = newPointsValue.ToString();
    }

    public void UpdateMovesText(int newPointsValue)
    {
        if (playerMovesText) playerMovesText.text = newPointsValue.ToString();
    }
}
