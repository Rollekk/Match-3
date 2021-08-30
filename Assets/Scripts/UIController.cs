using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Components")]
    Text playerPointsText;
    [SerializeField] Text playerMovesText;
    [SerializeField] Text plusText;

    [Header("Stats")]
    Vector3 plusTextPos; //initial plusText position
    int plusPoints = 0; //number of plus points to show

    private void Awake()
    {
        playerPointsText = GetComponentInChildren<Text>();
        plusText.enabled = false;
        plusTextPos = plusText.transform.position;
    }

    //Update UI points text
    public void UpdatePointsText(int newPointsValue)
    {
        if (playerPointsText) playerPointsText.text = newPointsValue.ToString();
    }

    //Update UI moves text
    public void UpdateMovesText(int newPointsValue)
    {
        if (playerMovesText) playerMovesText.text = newPointsValue.ToString();
    }

    //Update UI plus text
    public void UpdatePlusText(int newPointsValue)
    {
        //add all points to plusPoints
         plusPoints += newPointsValue;

        //check if plusText exists
        if(plusText)
        {
            //reset position
            plusText.transform.position = plusTextPos;
            //turn on
            plusText.enabled = true;
            //update text
            plusText.text = "+" + plusPoints.ToString();

            //Tween up with 60 units in 1 second
            LeanTween.moveLocalY(plusText.gameObject, 60.0f, 1f).setOnComplete(() =>
            {
                //turn off on complete
                plusText.enabled = false;
                //reset points
                plusPoints = 0;
            });
        }
    }
}
