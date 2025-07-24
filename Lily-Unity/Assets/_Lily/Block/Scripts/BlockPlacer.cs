using System.Collections.Generic;
using UnityEngine;

//Note: ブロックの配置可否や設置処理を行う
public class BlockPlacer : MonoBehaviour
{
    private BoardManager boardManager;
    private WallManager wallManager;

    private void Awake()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        wallManager = FindFirstObjectByType<WallManager>();
    }

    //Note: 指定のボード座標にブロックを配置できるかどうか
    public bool CanPlace(BlockController block, Vector2Int targetPos, Vector2Int moveDir)
    {
        List<Vector2Int> targetTiles = block.GetOccupiedTiles(targetPos);

        if (moveDir == Vector2Int.zero ||
        Mathf.Abs(moveDir.x) + Mathf.Abs(moveDir.y) != 1)
        {
            Debug.LogWarning($" 無効な移動方向: {moveDir}");
            return false;
        }

        foreach (var tile in targetTiles)
        {
            Vector2Int from = tile - moveDir;

            if (!boardManager.IsTIleWithinBounds(tile))
            {
                if (!wallManager.IsSameColorWall(from, moveDir, block.blockColor))
                {
                    Debug.LogWarning($" 枠外 + 色不一致 or 壁なし: {tile} ← {from} dir:{moveDir}");
                    return false;
                }
            }
            else
            {
                // 移動元からの方向に壁があるか
                if (wallManager.ExistsWall(from, moveDir) &&
                    !wallManager.IsSameColorWall(from, moveDir, block.blockColor))
                {
                    Debug.LogWarning($" 枠内だけど壁色不一致（進行方向）: {tile} ← {from} dir:{moveDir}");
                    return false;
                }

                // 移動先に、逆方向の壁があるか
                if (wallManager.ExistsWall(tile, -moveDir) &&
                    !wallManager.IsSameColorWall(tile, -moveDir, block.blockColor))
                {
                    Debug.LogWarning($" 枠内だけど壁色不一致（逆方向）: {tile} ← {from} dir:-{moveDir}");
                    return false;
                }
            }
        }

        if (IsOverlapping(block, targetTiles))
        {
            Debug.LogWarning(" 他ブロックと重なっている");
            return false;
        }

        Debug.Log(" 配置可能（CanPlace 通過）");
        return true;
    }


    private bool IsOverlapping(BlockController block, List<Vector2Int> targetTiles)
    {
        BlockController[] allBlocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);

        foreach (var other in allBlocks)
        {
            if (other == block)
            {
                continue;
            }

            foreach (var tile in other.GetOccupiedTiles())
            {
                if (targetTiles.Contains(tile))
                {
                    return true;
                }
            }
        }

        return false;
    }
}