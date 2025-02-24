using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockStaticController : Singleton<BlockStaticController>
{
    [SerializeField] private ColorSO blockColorSo;
    [SerializeField] private Transform parentBlockStaticTransform;

    private List<BlockStatic> _blockStaticList = new List<BlockStatic>();

    public List<BlockStatic> BlockStaticList => _blockStaticList;
    private void CreateStaticBlock(Vector2 position, Color color, int colorIndex)
    {
        BlockStatic blockStatic = SimplePool.Spawn<BlockStatic>
                                (PoolType.POOLTYPE_BLOCK_STATIC,
                                position,
                                Quaternion.identity,
                                parentBlockStaticTransform);
        blockStatic.OnInit(color, colorIndex);
        _blockStaticList.Add(blockStatic);
    }

    public void CreateAllStaticBlocks()
    {
        _blockStaticList.Clear();

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
                if (map[y][x] != 0)
                {
                    Vector2 position = new Vector2(startX + x * blockSize, startY - y * blockSize);
                    Color staticColor = blockColorSo.GetColor(colorIndex);
                    CreateStaticBlock(position, staticColor, colorIndex);
                }
            }
        }
    }

    public void DestroyAllStaticBlocks()
    {
        for (int i = 0; i < _blockStaticList.Count; i++)
        {
            SimplePool.Despawn(_blockStaticList[i]);
        }

        _blockStaticList.Clear();
    }
}

