using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemSO", menuName = "ScriptableObjects/GemSO")]
public class GemSO : ScriptableObject
{
    public enum EType { Normal, Bomb, ColorPicker }

    [SerializeField] private int gemPoints;
    [SerializeField] private EType gemType;
    [SerializeField] private Color gemColor;

    public int GetPoints => gemPoints;
    public EType GetEType => gemType;
    public Color GetColor => gemColor;
}
