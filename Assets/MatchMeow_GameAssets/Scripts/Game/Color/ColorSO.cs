using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ColorSO", menuName = "ScriptableObjects/ColorData")]
public class ColorSO : ScriptableObject
{
    [SerializeField] List<Color> colors = new List<Color>();

    public Color GetColor(int index) => colors[index];
}

