using System;
using System.Collections.Generic;
using UnityEngine;

public class BlockPathController : Singleton<BlockPathController>
{
    [SerializeField] private ColorSO blockColorSo;
    [SerializeField] private Transform pathBlockAncestor;

    private List<PathParent> _pathParents;

    public List<PathParent> PathParents => _pathParents;
    public void SpawnListPathParents(int maxValue)
    {
        _pathParents = new List<PathParent>(new PathParent[maxValue]);

        for (int i = 1; i <= maxValue; i++)
        {
            SpawnPathParent(i);
        }
    }

    private void SpawnPathParent(int colorIndex)
    {
        PathParent pathParent = SimplePool.Spawn<PathParent>(PoolType.POOLTYPE_PATH_PARENT, Vector2.zero, Quaternion.identity, pathBlockAncestor);
        pathParent.TF.name = "PathParent_" + colorIndex;
        _pathParents.Insert(colorIndex, pathParent);
    }

    public void SpawnPathBlock(Vector2 position, int colorIndex, Tuple<bool, bool, bool, bool> turn, BlockMovable blockMovable)
    {
        if (HasPathBlock(colorIndex, position))
        {
            return;
        }
        BlockPath pathBlock = SimplePool.Spawn<BlockPath>(PoolType.POOLTYPE_BLOCK_PATH, position, Quaternion.identity, _pathParents[colorIndex].TF);
        pathBlock.OnInit(colorIndex, blockColorSo.GetColor(colorIndex), turn, blockMovable);
        _pathParents[colorIndex].BlockPaths.Add(pathBlock);
    }

    public void RemovePathBlockOnScreen()
    {
        for (int i = 0; i < _pathParents.Count; i++)
        {
            if (_pathParents[i] != null)
            {
                for (int j = 0; j < _pathParents[i].BlockPaths.Count; j++)
                {
                    SimplePool.Despawn(_pathParents[i].BlockPaths[j]);
                }
                _pathParents[i].BlockPaths.Clear();
            }
        }
    }

    public void RemoveAllPathParent()
    {
        RemovePathBlockOnScreen();
        for (int i = 0; i < _pathParents.Count; i++)
        {
            if (_pathParents[i] != null)
            {
                SimplePool.Despawn(_pathParents[i]);
            }
        }
        _pathParents.Clear();
    }

    private bool HasPathBlock(int coLorIndex, Vector2 position)
    {
        for (int i = 0; i < _pathParents[coLorIndex].BlockPaths.Count; i++)
        {
            if (Vector2.Distance(_pathParents[coLorIndex].BlockPaths[i].TF.localPosition, position) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveAllPathBlock(int colorIndex)
    {
        for (int i = 0; i < _pathParents[colorIndex].BlockPaths.Count; i++)
        {
            SimplePool.Despawn(_pathParents[colorIndex].BlockPaths[i]);
        }
        _pathParents[colorIndex].BlockPaths.Clear();

        BlockManager.Instance.ResetCurrentMap(colorIndex);

        Debug.Log("RemoveAllPathBlock" + colorIndex);
    }

    public void RemovePathBlock(int colorIndex, Vector2 position)
    {
        for (int i = 0; i < _pathParents[colorIndex].BlockPaths.Count; i++)
        {
            if (Vector2.Distance(_pathParents[colorIndex].BlockPaths[i].TF.localPosition, position) < 0.1f)
            {
                SimplePool.Despawn(_pathParents[colorIndex].BlockPaths[i]);
                _pathParents[colorIndex].BlockPaths.RemoveAt(i);
                break;
            }
        }
    }
}

