using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockMovable : Block, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    private Vector2 _currentPos;
    private Vector2 _spawnPos;
    private float _movementExtraX;
    private float _movementExtraY;
    private float _blockSize;
    private int _colorIndex;

    private BlockMovable _anotherBlockMovable;
    private BlockStatic _blockStatic;

    private List<Vector2> _currentPath = new List<Vector2>();

    public BlockMovable AnotherBlockMovable
    {
        get => _anotherBlockMovable;
        set => _anotherBlockMovable = value;
    }

    public BlockStatic BlockStatic
    {
        get => _blockStatic;
        set => _blockStatic = value;
    }

    public int ColorIndex => _colorIndex;

    public List<Vector2> CurrentPath => _currentPath;

    public void OnInit(Color color, int colorIndex, Vector2 spawnPosition)
    {
        base.OnInit(color);
        _colorIndex = colorIndex;
        _spawnPos = spawnPosition;
        _currentPos = new Vector2(spawnPosition.x, spawnPosition.y);
        _blockSize = Constance.InGameObject.BLOCK_SIZE;
        _currentPath = new List<Vector2>();
        int mapWidth = LevelManager.Instance.CurrentLevel.Width;
        int mapHeight = LevelManager.Instance.CurrentLevel.Height;
        _movementExtraX = (mapWidth/2f - 0.5f - Mathf.Floor(mapWidth/2f)) * _blockSize;
        _movementExtraY = (mapHeight/2f - 0.5f - Mathf.Floor(mapHeight/2f)) * _blockSize;
        // _movementExtraY = 0;
    }

    public void MoveBlock(Vector2 mousePos2D)
    {
        BlockManager.Instance.CurrentColorIndex = _colorIndex;
        Tuple<Vector2, Vector2, int, int> startTargetPos = GetStartTargetPos(mousePos2D);
        Vector2 startPos = startTargetPos.Item1;
        Vector2 targetPos = startTargetPos.Item2;

        if (!CanFindPath(startTargetPos))
        {
            return;
        }

        List<Vector2> path = BlockManager.Instance.FindPathBFS(startPos, targetPos, _colorIndex);

        if (CanDrawPath(path))
        {
            DrawPath(path);
        }
        // BlockManager.Instance.ChangeCurrentMap(_colorIndex, _currentPath);
    }

    private void DrawPath(List<Vector2> path)
    {

        // Debug.Log("DrawPath" + path.Count);
        for (int i = 1; i < path.Count; i++)
        {

            if (Vector2.Distance(BlockManager.Instance.ChoosenBlocks[_colorIndex].TF.localPosition,
                    BlockManager.Instance.ChoosenBlocks[_colorIndex].AnotherBlockMovable.TF.localPosition) < 0.1f)
            {
                return;
            }

            bool isTurnRight = path[i - 1].y < path[i].y;
            bool isTurnLeft = path[i - 1].y > path[i].y;
            bool isTurnUp = path[i - 1].x > path[i].x;
            bool isTurnDown = path[i - 1].x < path[i].x;
            // Tuple<bool, bool, bool, bool> turn = new Tuple<bool, bool, bool, bool>(isTurnRight, isTurnLeft, isTurnUp, isTurnDown);

            Tuple<bool, bool, bool, bool> turn = new Tuple<bool, bool, bool, bool>(isTurnLeft, isTurnRight, isTurnDown, isTurnUp);
            float posX = path[i - 1].y * _blockSize + BlockManager.Instance.StartX;
            float posY = -path[i - 1].x * _blockSize + BlockManager.Instance.StartY;
            Vector2 position = new Vector2(posX, posY);

            // spawn path block
            BlockPathController.Instance.SpawnPathBlock(position, _colorIndex, turn, this);

            _currentPath.Add(path[i]);

            float objectNewPosX = path[i].y * _blockSize + BlockManager.Instance.StartX;
            float objectNewPosy = -path[i].x * _blockSize + BlockManager.Instance.StartY;
            Vector2 objectNewPos = new Vector2(objectNewPosX, objectNewPosy);
            TF.localPosition = objectNewPos;
            _currentPos = objectNewPos;
        }
    }

    private bool CanDrawPath(List<Vector2> path)
    {
        int minIndexFound = Math.Max(_currentPath.Count, path.Count);
        for (int i = 1; i < path.Count; i++)
        {
            int index = _currentPath.IndexOf(path[i]);
            if (index != -1)
            {
                minIndexFound = Math.Min(minIndexFound, index);
            }
        }

        Debug.Log("minIndexFound" + minIndexFound + " _currentPath.Count - 1 " + (_currentPath.Count - 1));
        if (minIndexFound < _currentPath.Count - 1 && minIndexFound >= 0 && path != _currentPath)
        {
            for (int i = _currentPath.Count - 1; i > minIndexFound; i--)
            {
                // remove path block
                Debug.Log("RemovePathBlock" + _currentPath[i - 1].x.ToString() +  _currentPath[i - 1].y.ToString());
                RemovePathBlock(_currentPath[i - 1].x, _currentPath[i - 1].y);
                _currentPath.RemoveAt(i);

                float posX = _currentPath[i - 1].y * _blockSize + BlockManager.Instance.StartX;
                float posY = -_currentPath[i - 1].x * _blockSize + BlockManager.Instance.StartY;

                Vector2 position = new Vector2(posX, posY);
                TF.localPosition = position;
                _currentPos = position;
            }
            return false;
        }
        else
        {
            if (minIndexFound == 0)
            {
                // resetPath
                ResetPath();
                return false;
            }
        }
        return true;
    }

    public void ResetPath()
    {
        if (BlockManager.Instance.CurrentColorIndex == 0) return;
        BlockPathController.Instance.RemoveAllPathBlock(_colorIndex);
        ResetMovableBlock();
    }

    private void ResetMovableBlock()
    {
        TF.localPosition = _spawnPos;
        _currentPos = _spawnPos;

        int indexRow = (int) Mathf.Round((BlockManager.Instance.StartY - _spawnPos.y) / _blockSize);
        int indexCol = (int) Mathf.Round((_spawnPos.x - BlockManager.Instance.StartX) / _blockSize);

        _currentPath.Clear();
        _currentPath.Add(new Vector2(indexRow, indexCol));
    }

    private void RemovePathBlock(float pathX, float pathY)
    {
        float posX = pathY * _blockSize + BlockManager.Instance.StartX;
        float posY = -pathX * _blockSize + BlockManager.Instance.StartY;

        Vector2 position = new Vector2(posX, posY);

        // remove path block
        BlockPathController.Instance.RemovePathBlock(_colorIndex, position);
        // 13/2/ 5:40
        // _currentPath.Remove(new Vector2(pathX, pathY));
    }

    private bool CanFindPath(Tuple<Vector2, Vector2, int, int> startTargetPos)
    {
        // startPos, targetPos, targetIndexRow, targetIndexCol => item1, item2, item3, item4
        if (startTargetPos.Item3 < 0 ||
            startTargetPos.Item3 >= BlockManager.Instance.FixedMap.Length ||
            startTargetPos.Item4 < 0 ||
            startTargetPos.Item4 >= BlockManager.Instance.FixedMap[0].Length)
        {
            return false;
        }

        int colorNumber = _colorIndex;
        int number = BlockManager.Instance.CurrentMap[startTargetPos.Item3][startTargetPos.Item4];
        if (number != 0 && number != colorNumber)
        {
            return false;
        }
        return true;
    }

    private Tuple<Vector2, Vector2, int, int> GetStartTargetPos(Vector2 delta)
    {
        float posX = Mathf.Round((delta.x - _movementExtraX) / _blockSize) * _blockSize + _movementExtraX;
        float posY = Mathf.Round((delta.y - _movementExtraY) / _blockSize) * _blockSize + _movementExtraY;
        Debug.Log("posX" + posX + "posY" + posY);
        int targetIndexRow = (int) Mathf.Round((BlockManager.Instance.StartY - posY) / _blockSize);
        int targetIndexCol = (int) Mathf.Round((posX - BlockManager.Instance.StartX) / _blockSize);

        // Debug.Log("CurrentPos" + currentPos);
        Vector2 startPos1 = new Vector2(_currentPos.x, _currentPos.y);
        int startIndexRow = (int) Mathf.Round((BlockManager.Instance.StartY - startPos1.y) / _blockSize);
        int startIndexCol = (int) Mathf.Round((startPos1.x - BlockManager.Instance.StartX) / _blockSize);
        Vector2 startPos = new Vector2(startIndexRow, startIndexCol);
        Vector2 targetPos = new Vector2(targetIndexRow, targetIndexCol);

        Debug.Log("start: " + startPos + " target: " + targetPos);
        return new Tuple<Vector2, Vector2, int, int>(startPos, targetPos, targetIndexRow, targetIndexCol);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("MovableBlock OnPointerDown");
        BlockManager.Instance.CurrentColorIndex = _colorIndex;
        if (_currentPath.Count == 0)
        {
            int posX = (int) Mathf.Round((_currentPos.x - BlockManager.Instance.StartX) / _blockSize);
            int posY = (int) Mathf.Round((BlockManager.Instance.StartY - _currentPos.y) / _blockSize);
            _currentPath.Add(new Vector2(posY, posX));
        }

        //isChoosing = true;

        // before
        if (!BlockManager.Instance.ChoosenBlocks.ContainsKey(_colorIndex))
        {
            BlockManager.Instance.ChoosenBlocks.Add(_colorIndex, this);
            //Debug.Log(this);
        }
        else
        {
            if (BlockManager.Instance.ChoosenBlocks[_colorIndex] == null)
            {
                BlockManager.Instance.ChoosenBlocks[_colorIndex] = this;
            }
        }
        // BlockMovable block = BlockManager.Instance.ChoosenBlocks[_colorIndex];
        // // block.ResetPath();
        // if (block != this)
        // {
        //     if (block != null && block.ColorIndex == _colorIndex)
        //     {
        //         // this reset path
        //         block.ResetPath();
        //         // block.resetBlock
        //         // block.ResetMovableBlock();
        //         // reset Character
        //     }
        //     BlockManager.Instance.ChoosenBlocks[_colorIndex] = this;
        // }
        if (BlockManager.Instance.ChoosenBlocks[_colorIndex] != this)
        {
            BlockManager.Instance.ChoosenBlocks[_colorIndex] = this;
            BlockManager.Instance.ChoosenBlocks[_colorIndex].AnotherBlockMovable.ResetPath();
        }


        // //after
        // if (!BlockManager.Instance.ChoosenBlocks.ContainsKey(_colorIndex))
        // {
        //     BlockManager.Instance.ChoosenBlocks[_colorIndex] = this;
        //     return;
        // }
        //
        // if (BlockManager.Instance.ChoosenBlocks[_colorIndex] != this)
        // {
        //     BlockManager.Instance.ChoosenBlocks[_colorIndex].ResetPath();
        //     BlockManager.Instance.ChoosenBlocks[_colorIndex] = this;
        // }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPointerUp();
    }

    public void OnPointerUp()
    {
        // Debug.Log("MovableBlock OnPointerUp");

        _currentPos = TF.localPosition;
         BlockManager.Instance.ChangeCurrentMap(_colorIndex);
         Debug.Log("CurrentMap:" + BlockManager.Instance.MatrixToString(BlockManager.Instance.CurrentMap));
         // Debug.Log(BlockManager.Instance.CurrentMap == BlockManager.Instance.FixedMap);
        // isChoosing = false;
        // check win
        // make hero attack monster
        // Debug.Log("CurrentPath" + currentPath);
        // BlockManager.Instance.MakeCatJumpToFireFly(this, _colorIndex);
        bool isWin = BlockManager.Instance.CheckWin();
        if (isWin)
        {
            GameManager.Instance.OnWinState();
            Debug.Log("Win");
            return;
        }

        if (Vector2.Distance(TF.localPosition, _anotherBlockMovable.BlockStatic.TF.localPosition) < 0.1f)
        {
            SoundManager.Instance.PlayScoreSound();
            VibrationManager.Instance.Vibrate();
        }

        if (Vector2.Distance(TF.localPosition, _blockStatic.TF.localPosition) < 0.1f)
        {
            if (_blockStatic.BoxCollider2D.enabled)
            {
                _blockStatic.BoxCollider2D.enabled = false;
            }
        }
        else
        {
            if (!_blockStatic.BoxCollider2D.enabled)
            {
                _blockStatic.BoxCollider2D.enabled = true;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        BlockManager.Instance.CurrentColorIndex = _colorIndex;
        // if (_currentPath.Count == 0)
        // {
        //     int posX = (int) Mathf.Round((_currentPos.x - BlockManager.Instance.StartX) / _blockSize);
        //     int posY = (int) Mathf.Round((BlockManager.Instance.StartY - _currentPos.y) / _blockSize);
        //     _currentPath.Add(new Vector2(posY, posX));
        // }
        //
        // Dictionary<int, BlockMovable> choosenBlocks = BlockManager.Instance.ChoosenBlocks;
        //
        // // before
        // if (!BlockManager.Instance.ChoosenBlocks.ContainsKey(_colorIndex))
        // {
        //     BlockManager.Instance.ChoosenBlocks.Add(_colorIndex, this);
        //     //Debug.Log(this);
        // }
        // else
        // {
        //     if (BlockManager.Instance.ChoosenBlocks[_colorIndex] == null)
        //     {
        //         BlockManager.Instance.ChoosenBlocks[_colorIndex] = this;
        //     }
        // }
        // BlockMovable block = BlockManager.Instance.ChoosenBlocks[_colorIndex];
        // // block.ResetPath();
        // if (block != this)
        // {
        //     if (block != null && block.ColorIndex == _colorIndex)
        //     {
        //         // this reset path
        //         block.ResetPath();
        //         // block.resetBlock
        //         // block.ResetMovableBlock();
        //         // reset Character
        //     }
        //     BlockManager.Instance.ChoosenBlocks[_colorIndex] = this;
        // }

        // Debug.Log("MovableBlock OnDrag");
        // Vector3 delta = eventData.pressPosition; // thử convert

        Vector3 screenPosition = eventData.position; // Lấy vị trí trên màn hình
        screenPosition.z = Camera.main.nearClipPlane; // Đặt khoảng cách từ camera (quan trọng)

        Vector3 delta = Camera.main.ScreenToWorldPoint(screenPosition);
        // Vector2 delta2D = new Vector2(delta.x - Constance.ScreenInfo.SCREEN_WIDTH/2, delta.y - Constance.ScreenInfo.SCREEN_HEIGHT/2);
        // Debug.Log(delta + " " + delta2D);
        Vector2 delta2D = new Vector2(delta.x, delta.y);
        Debug.Log(delta2D);
        // BlockMovable block = BlockManager.Instance.ChoosenBlocks[BlockManager.Instance.CurrentColorIndex];
        // if (block == this)
        // {
        //     // Debug.Log("MovableBlock OnDrag block == this");
        //     MoveBlock(delta2D);
        // }
        BlockManager.Instance.ChoosenBlocks[_colorIndex].MoveBlock(delta2D);
        // else
        // {
        //     if (block != null)
        //     {
        //         anotherBlock.MoveBlock(delta2D);
        //     }
        // }
        //MoveBlock(delta2D);

        // _currentPos = TF.localPosition;
        // BlockManager.Instance.ChangeCurrentMap(_colorIndex, _currentPath);
        // // Debug.Log(BlockManager.Instance.CurrentMap == BlockManager.Instance.FixedMap);
        // // isChoosing = false;
        // // check win
        // // make hero attack monster
        // // Debug.Log("CurrentPath" + currentPath);
        // // BlockManager.Instance.MakeCatJumpToFireFly(this, _colorIndex);
        // bool isWin = BlockManager.Instance.CheckWin();
        // if (isWin)
        // {
        //     Debug.Log("Win");
        //     return;
        // }
        //
        // if (Vector2.Distance(TF.localPosition, _blockStatic.TF.localPosition) < 0.1f)
        // {
        //     if (_blockStatic.BoxCollider2D.enabled)
        //     {
        //         _blockStatic.BoxCollider2D.enabled = false;
        //     }
        // }
        // else
        // {
        //     if (!_blockStatic.BoxCollider2D.enabled)
        //     {
        //         _blockStatic.BoxCollider2D.enabled = true;
        //     }
        // }
    }
}

