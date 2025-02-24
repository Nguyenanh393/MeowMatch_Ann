using UnityEngine;
using UnityEngine.EventSystems;

public class BlockStatic : Block, IPointerDownHandler //, IDragHandler //, IPointerUpHandler
{
    private int _colorIndex;
    [SerializeField] private BoxCollider2D _boxCollider2D;
    private BlockMovable _blockMovable;

    public BoxCollider2D BoxCollider2D => _boxCollider2D;
    public BlockMovable BlockMovable
    {
        get => _blockMovable;
        set => _blockMovable = value;
    }

    public void OnInit(Color color, int colorIndex)
    {
        base.OnInit(color);
        _colorIndex = colorIndex;
        _boxCollider2D.enabled = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        BlockManager.Instance.CurrentColorIndex = _colorIndex;
        if (BlockManager.Instance.ChoosenBlocks.ContainsKey(_colorIndex))
        {
            BlockManager.Instance.ChoosenBlocks[_colorIndex].ResetPath();
            _boxCollider2D.enabled = false;
        }
        BlockManager.Instance.ChangeCurrentMap(_colorIndex);
    }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     BlockManager.Instance.CurrentColorIndex = _colorIndex;
    //     if (_blockMovable == null || _blockMovable != BlockManager.Instance.ChoosenBlocks[_colorIndex])
    //     {
    //         BlockManager.Instance.ChoosenBlocks[_colorIndex].ResetPath();
    //         BlockManager.Instance.ChoosenBlocks[_colorIndex] = _blockMovable;
    //     }
    //     _blockMovable.OnDrag(eventData);
    // }

    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     if (_blockMovable != null)
    //     {
    //         _blockMovable.OnPointerUp();
    //     }
    //
    // }
}

