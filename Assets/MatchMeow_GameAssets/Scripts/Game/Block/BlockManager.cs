using System;
using System.Collections.Generic;
using UnityEngine;
public class BlockManager :  Singleton<BlockManager>
{
    private float _startX;
    private float _startY;

    private Level _currentLevel;

    private int _mapWidth;
    private int _mapHeight;
    private int[][] _currentMap;
    private int[][] _fixedMap;
    private int _currentColorIndex;

    private Dictionary<int, BlockMovable> _choosenBlocks = new Dictionary<int, BlockMovable>();
    // private Dictionary<>
    public float StartX => _startX;
    public float StartY => _startY;

    public int[][] CurrentMap { get => _currentMap; set => _currentMap = value; }
    public int[][] FixedMap { get => _fixedMap; set => _fixedMap = value; }
    public int CurrentColorIndex { get => _currentColorIndex; set => _currentColorIndex = value; }
    public Dictionary<int, BlockMovable> ChoosenBlocks { get => _choosenBlocks; set => _choosenBlocks = value; }

    private void Awake()
    {
        Constance.ScreenInfo.SCREEN_HEIGHT = UIManager.Instance.CanvasParentTF.gameObject
            .GetComponent<RectTransform>().rect.height;
        Constance.ScreenInfo.SCREEN_WIDTH = UIManager.Instance.CanvasParentTF.gameObject
            .GetComponent<RectTransform>().rect.width;

        Debug.Log(Constance.ScreenInfo.SCREEN_WIDTH + " " + Constance.ScreenInfo.SCREEN_HEIGHT);
    }

    private void Start()
    {
        OnInit();
        LoadMapBoard();
    }

    public void OnInit()
    {
        _currentColorIndex = 0;
        _currentLevel = LevelManager.Instance.CurrentLevel;

        _mapWidth = _currentLevel.Width;
        _mapHeight = _currentLevel.Height;

        _startX = (- _mapWidth / 2f + 0.5f) * Constance.InGameObject.BLOCK_SIZE;
        _startY = (_mapHeight / 2f - 0.5f) * Constance.InGameObject.BLOCK_SIZE - 1;

        _currentMap = DeepClone(_currentLevel.Map);
        _fixedMap = DeepClone(_currentLevel.Map);

        _choosenBlocks.Clear();
    }

    public void LoadMapBoard()
    {
        BlockFloorController.Instance.CreateFloor();
        BlockStaticController.Instance.CreateAllStaticBlocks();
        BlockMovableController.Instance.CreateAllMovableBlocks();

        BlockPathController.Instance.SpawnListPathParents(LevelManager.Instance.CurrentLevel.MaxValue);
        SetAllInitBlocks();
    }

    [ContextMenu("RemoveAllMapBoard")]
    public void RemoveAllMapBoard()
    {
        BlockPathController.Instance.RemoveAllPathParent();
        BlockFloorController.Instance.RemoveFloor();
        BlockStaticController.Instance.DestroyAllStaticBlocks();
        BlockMovableController.Instance.RemoveAllMovableBlocks();
    }

    private void SetAllInitBlocks()
    {
        BlockMovableController.Instance.SetAnotherBlockMovablePair();
        SetMovableStaticEveryBlock();
    }

    private void SetMovableStaticEveryBlock()
    {
        for (int i = 0; i < BlockStaticController.Instance.BlockStaticList.Count; i++)
        {
            BlockStatic blockStatic = BlockStaticController.Instance.BlockStaticList[i];
            BlockMovable blockMovable = BlockMovableController.Instance.BlockMovableList[i];

            blockMovable.BlockStatic = blockStatic;
            blockStatic.BlockMovable = blockMovable;
        }
    }

    public List<Vector2> FindPathBFS(Vector2 startPos, Vector2 targetPos, int colorIndex)
    {
        int colorNumber = colorIndex;
        int rows = LevelManager.Instance.CurrentLevel.Height;
        int cols = LevelManager.Instance.CurrentLevel.Width;
        Queue<Vector2> queue = new Queue<Vector2>();
        HashSet<string> visited = new HashSet<string>();
        Dictionary<string, Vector2> parentMap = new Dictionary<string, Vector2>();

        queue.Enqueue(startPos);
        visited.Add($"{startPos.x},{startPos.y}");
        parentMap.Add($"{startPos.x},{startPos.y}", new Vector2(-1, -1));

        while (queue.Count > 0)
        {
            Vector2 current = queue.Dequeue();
            int x = (int) current.x;
            int y = (int) current.y;

            if (Math.Abs(x - targetPos.x) < 0.1f && Math.Abs(y - targetPos.y) < 0.1f)
            {
                return ConstructPath(parentMap, current);
            }

            int[][] directions = new int[][]
            {
                new int[] { 1, 0 },  // Right
                new int[] { 0, 1 },  // Down
                new int[] { -1, 0 }, // Left
                new int[] { 0, -1 }  // Up
            };

            foreach (var direction in directions)
            {
                int nx = x + direction[0];
                int ny = y + direction[1];
                string key = $"{nx},{ny}";

                if (nx >= 0 && nx < rows && ny >= 0 && ny < cols && !visited.Contains(key))
                {
                    if (_currentMap[nx][ny] == 0 || _currentMap[nx][ny] == colorNumber)
                    {
                        // Debug.Log(_currentMap[nx][ny] == colorNumber);
                        visited.Add(key);
                        queue.Enqueue(new Vector2(nx, ny));
                        parentMap[key] = new Vector2(x, y);
                    }
                }
            }
        }
        return new List<Vector2>();
    }

    private List<Vector2> ConstructPath(Dictionary<string, Vector2> parentMap, Vector2 endPos)
    {
        List<Vector2> path = new List<Vector2>();
        Vector2 step = endPos;
        int count = 0;

        while (step.x > -1 && step.y > -1)
        {
            if (count > 100)
            {
                Debug.LogError("Infinite loop in ConstructPath");
                break;
            }
            count++;

            path.Insert(0, new Vector2(step.x, step.y)); // Add to the front of the path list
            parentMap.TryGetValue($"{step.x},{step.y}", out step);
        }
        return path;
    }

    public void ResetCurrentMap(int colorIndex)
    {
        for (int i = 0; i < _fixedMap.Length; i++)
        {
            for (int j = 0; j < _fixedMap[i].Length; j++)
            {
                if (_currentMap[i][j] == colorIndex && _fixedMap[i][j] == 0)
                {
                    _currentMap[i][j] = 0;
                }
            }
        }
    }

    public void ChangeCurrentMap(int colorIndex)
    {
        ResetCurrentMap(colorIndex);
        for (int i = 0; i < BlockPathController.Instance.PathParents[colorIndex].BlockPaths.Count; i++)
        {
            Vector2 position = BlockPathController.Instance.PathParents[colorIndex].BlockPaths[i].TF.localPosition;
            Vector2 mapPos = GetMapPosition(position);
            _currentMap[(int) mapPos.x][(int) mapPos.y] = colorIndex;
        }

        for (int i = 0; i < BlockMovableController.Instance.BlockMovableList.Count; i++)
        {
            BlockMovable blockMovable = BlockMovableController.Instance.BlockMovableList[i];
            if (blockMovable.ColorIndex != colorIndex) continue;
            Vector2 position = blockMovable.TF.localPosition;
            Vector2 mapPos = GetMapPosition(position);
            _currentMap[(int) mapPos.x][(int) mapPos.y] = colorIndex;
        }

        Debug.Log(MatrixToString(_currentMap));
        // Debug.Log(MatrixToString(_fixedMap));
    }

    private T[][] DeepClone<T>(T[][] original)
    {
        if (original == null) return null;

        var length = original.Length;
        var clone = new T[length][];
        for (int i = 0; i < length; i++)
        {
            if (original[i] != null)
            {
                clone[i] = new T[original[i].Length];
                Array.Copy(original[i], clone[i], original[i].Length);
            }
        }
        return clone;
    }

    public bool CheckWin()
    {
        Dictionary<int, BlockMovable> choosenBlockMovables = _choosenBlocks;
        for (int i = 1; i <= LevelManager.Instance.CurrentLevel.MaxValue; i++)
        {
            if (!choosenBlockMovables.ContainsKey(i)) return false;
            BlockMovable blockMovable = choosenBlockMovables[i];
            if (Vector3.Distance(blockMovable.TF.position, blockMovable.AnotherBlockMovable.BlockStatic.TF.position) >= 0.1f)
            {
                return false;
            }
        }
        return true;

        // return false;
    }

    public string MatrixToString(int[][] matrix)
    {
        string result = "[";
        int rows = matrix.Length;
        int cols = matrix[0].Length;

        for (int i = 0; i < rows; i++)
        {
            result += "[";
            for (int j = 0; j < cols; j++)
            {
                result += matrix[i][j];
                if (j < cols - 1) result += ", "; // Ngăn cách giữa các số
            }
            result += "]";
            if (i < rows - 1) result += ", "; // Ngăn cách giữa các hàng
        }

        result += "]";
        return result;
    }

    public Vector2 GetMapPosition(Vector2 currentPos)
    {
        int posX = (int) Mathf.Round((currentPos.x - _startX) / Constance.InGameObject.BLOCK_SIZE);
        int posY = (int) Mathf.Round((_startY - currentPos.y) / Constance.InGameObject.BLOCK_SIZE);
        return (new Vector2(posY, posX));
    }
}

