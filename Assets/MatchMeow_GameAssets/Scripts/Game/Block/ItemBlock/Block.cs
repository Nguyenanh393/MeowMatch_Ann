using System.Collections;
using System.Collections.Generic;
using _Pool.Pool;
using UnityEngine;
using UnityEngine.UI;

public class Block : PoolUnit
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _blockColor;
    private void SetColor(Color color)
    {
        _blockColor = color;
        _spriteRenderer.color = color;
    }
    public Color BlockColor => _blockColor;

    public void OnInit(Color color)
    {
        SetColor(color);
        TF.localScale = Vector3.one * Constance.InGameObject.BLOCK_SIZE;
    }
}

