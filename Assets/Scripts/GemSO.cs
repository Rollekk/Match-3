using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemSO", menuName = "ScriptableObjects/GemSO")]
public class GemSO : ScriptableObject
{
    public enum EType { Special, Normal }

    [SerializeField] private int gemPoints;
    [SerializeField] private Color gemColor;
    [SerializeField] private EType gemType;

    public int GetPoints => gemPoints;
    public Color GetColor => gemColor;
    public EType GetEType => gemType;
}
