using System.Collections;
using System.Collections.Generic;
using _Pool.Pool;
using UnityEngine;

public class PathParent : PoolUnit
{
    private List<BlockPath> blockPaths = new List<BlockPath>();

    public List<BlockPath> BlockPaths => blockPaths;
}
