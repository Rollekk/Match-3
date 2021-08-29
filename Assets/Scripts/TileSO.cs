using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GemSO", menuName = "ScriptableObjects/GemSO")]
public class TileSO : ScriptableObject
{
    public enum EType { Normal, Bomb, ColorPicker }

    [Header("Stats")]
    [SerializeField] private int tilePoints;
    [SerializeField] private EType tileType;
    [SerializeField] private Color tileColor;
    [SerializeField] private Sprite tileSprite;

    public int GetPoints => tilePoints;
    public EType GetEType => tileType;
    public Color GetColor => tileColor;
    public Sprite GetSprite => tileSprite;
}
