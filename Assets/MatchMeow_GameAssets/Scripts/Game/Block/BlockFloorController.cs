using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockFloorController : Singleton<BlockFloorController>
{
    [SerializeField] private ColorSO floorColorSo;
    [SerializeField] private Transform parentBlockFloorTransform;

    private List<BlockFloor> _blockFloorList = new List<BlockFloor>();

    private void CreateFloorBlock(Vector2 position, Color color)
    {
        BlockFloor blockFloor = SimplePool.Spawn<BlockFloor>
                                (PoolType.POOLTYPE_BLOCK_FLOOR,
                                position,
                                Quaternion.identity,
                                parentBlockFloorTransform);
        blockFloor.OnInit(color);
        _blockFloorList.Add(blockFloor);
    }

    public void CreateFloor()
    {
        _blockFloorList.Clear();

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
                Vector2 position = new Vector2(startX + x * blockSize, startY - y * blockSize);
                int colorIndex = (x + y) % 2;
                Color floorColor = floorColorSo.GetColor(colorIndex);
                CreateFloorBlock(position, floorColor);
            }
        }
    }

    public void RemoveFloor()
    {
        for (int i = 0; i < _blockFloorList.Count; i++)
        {
            SimplePool.Despawn(_blockFloorList[i]);
        }

        _blockFloorList.Clear();
    }
}

