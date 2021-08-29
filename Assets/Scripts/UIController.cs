using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Components")]
    Text playerPointsTMP;

    private void Awake()
    {
        playerPointsTMP = GetComponentInChildren<Text>();
    }

    public void UpdatePointsText(int newPointsValue)
    {
        if(playerPointsTMP) playerPointsTMP.text = newPointsValue.ToString();
    }
}
