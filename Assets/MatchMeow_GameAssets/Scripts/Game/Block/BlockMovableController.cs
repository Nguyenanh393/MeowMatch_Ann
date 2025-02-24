using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
public class BlockMovableController :Singleton<BlockMovableController>
{
    [SerializeField] private ColorSO blockColorSo;
    [SerializeField] private Transform parentBlockMovableTransform;

    private List<BlockMovable> _blockMovableList = new List<BlockMovable>();

    public List<BlockMovable> BlockMovableList => _blockMovableList;

    private void CreateMovableBlock(Vector2 position, Color color, int colorIndex)
    {
        BlockMovable blockMovable = SimplePool.Spawn<BlockMovable>
        (PoolType.POOLTYPE_BLOCK_MOVABLE,
            position,
            Quaternion.identity,
            parentBlockMovableTransform);
        blockMovable.OnInit(color, colorIndex, position);
        _blockMovableList.Add(blockMovable);
    }

    public void CreateAllMovableBlocks()
    {
        _blockMovableList.Clear();

        float blockSize = Constance.InGameObject.BLOCK_SIZE;
        float startX = BlockManager.Instance.StartX;
        float startY = BlockManager.Instance.StartY;

        int[][] map = BlockManager.Instance.FixedMap;
        int mapWidth = map[0].Length;
        int mapHeight = map.Length;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int colorIndex = map[y][x];
                if (colorIndex != 0)
                {
                    Vector2 position = new Vector2(startX + x * blockSize, startY - y * blockSize);
                    Color staticColor = blockColorSo.GetColor(colorIndex);
                    CreateMovableBlock(position, staticColor, colorIndex);
                }
            }
        }
    }

    public void RemoveAllMovableBlocks()
    {
        for (int i = 0; i < _blockMovableList.Count; i++)
        {
            SimplePool.Despawn(_blockMovableList[i]);
        }

        _blockMovableList.Clear();
    }

    public void SetAnotherBlockMovablePair()
    {
        var groupedByColorId = _blockMovableList.GroupBy(item => item.ColorIndex);
        foreach (var group in groupedByColorId)
        {
            var pairList = group.ToList();
            if (pairList.Count == 2) // Chỉ xử lý khi có đúng 2 object cùng colorId
            {
                pairList[0].AnotherBlockMovable = pairList[1];
                pairList[1].AnotherBlockMovable = pairList[0];
            }
            else
            {
                Debug.LogError($"ColorId {group.Key} không có đúng 2 object!");
            }
        }
    }
}

