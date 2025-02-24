using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockPath : Block, IPointerDownHandler//, IDragHandler //, IPointerUpHandler
{
    private int _colorIndex;
    private BlockMovable _movableBlock;
    public void OnInit(int colorIndex, Color color, Tuple<bool, bool, bool, bool> turn, BlockMovable movableBlock)
    {
        _colorIndex = colorIndex;
        _movableBlock = movableBlock;
        base.OnInit(color);
        SetTurn(turn);
    }
    private void SetTurn(Tuple<bool, bool, bool, bool> turn)
    {
        float angle = 0;

        if (turn.Item1)
        {
            angle = 90;
        } else if (turn.Item2)
        {
            angle = 270;
        } else if (turn.Item3)
        {
            angle = 180;
        }
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BlockManager.Instance.CurrentColorIndex = _colorIndex;
        Debug.Log("BlockPath.OnPointerDown");
        Vector3 screenPosition = eventData.position; // Lấy vị trí trên màn hình
        screenPosition.z = Camera.main.nearClipPlane; // Đặt khoảng cách từ camera (quan trọng)

        Vector3 delta = Camera.main.ScreenToWorldPoint(screenPosition);
        // Vector2 delta2D = new Vector2(delta.x - Constance.ScreenInfo.SCREEN_WIDTH/2, delta.y - Constance.ScreenInfo.SCREEN_HEIGHT/2);
        // Debug.Log(delta + " " + delta2D);
        Vector2 delta2D = new Vector2(delta.x, delta.y);
        BlockManager.Instance.ChoosenBlocks[BlockManager.Instance.CurrentColorIndex] = _movableBlock;
        _movableBlock.MoveBlock(delta2D);

        // BlockManager.Instance.ChoosenBlocks[_colorIndex] = _movableBlock;
        // BlockManager.Instance.CurrentColorIndex = _colorIndex;
        // BlockManager.Instance.ChangeCurrentMap(_colorIndex, _movableBlock.CurrentPath);
        // moveMonster
        // UIManager.Instance.GetUI<GamePlayUI>().StarArea.ReturnStar();
        BlockManager.Instance.ChangeCurrentMap(_colorIndex);

    }

    // public void OnDrag(PointerEventData eventData)
    // {
    //     // Debug.Log("BlockPath.OnDrag");
    //     // BlockManager.Instance.CurrentColorIndex = _colorIndex;
    //     // if (_movableBlock == null || _movableBlock != BlockManager.Instance.ChoosenBlocks[_colorIndex])
    //     // {
    //     //     // _movableBlock = BlockManager.Instance.ChoosenBlocks[_colorIndex];
    //     //     BlockManager.Instance.ChoosenBlocks[BlockManager.Instance.CurrentColorIndex] = _movableBlock;
    //     // }
    //     // _movableBlock.OnDrag(eventData);
    //     OnPointerDown(eventData);
    //     BlockManager.Instance.ChoosenBlocks[BlockManager.Instance.CurrentColorIndex].OnDrag(eventData);
    // }


    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     if (_movableBlock != null)
    //     {
    //         _movableBlock.OnPointerUp();
    //     }
    // }
}

