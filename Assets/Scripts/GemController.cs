using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GemSO gemStats;

    [Header("Stats")]
    private int points;
    private Color color;
    private GemSO.EType type;

    // Start is called before the first frame update
    void Start()
    {
        points = gemStats.GetPoints;
        color = gemStats.GetColor;
        type = gemStats.GetEType;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
