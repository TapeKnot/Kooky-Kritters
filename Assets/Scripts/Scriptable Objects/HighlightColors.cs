using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "HighlightColors", menuName = "Scriptable Objects/HighlightColors")]
public class HighlightColors : ScriptableObject
{
    public List<Color> colors;
}
