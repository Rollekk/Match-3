using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemSO", menuName = "ScriptableObjects/GemSO")]
public class GemSO : ScriptableObject
{
    public enum EType { Normal, Bomb, ColorPicker }

    [SerializeField] private int gemPoints;
    [SerializeField] private Color gemColor;
    [SerializeField] private EType gemType;

    public int GetPoints => gemPoints;

    //Get random gem type
    public EType GetEType()
    {
        float randomProb = Random.Range(0f, 100f);

        if (randomProb <= 95f) return gemType = EType.Normal;
        else return gemType = (EType) Random.Range(1, 2);
    }

    //Get color based on gem type
    public Color GetColor()
    {
        if (gemType == EType.Normal) return gemColor;
        else if (gemType == EType.Bomb) return Color.black;
        else return gemColor + Color.gray;
    }
}
