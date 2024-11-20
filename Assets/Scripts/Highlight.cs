using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Highlight : MonoBehaviour
{
    [SerializeField] private HighlightColors highlightColors;
    [SerializeField] private SpriteRenderer sprite;

    public void SetColor(int idx)
    {
        sprite.color = highlightColors.colors[idx];
    }
}
