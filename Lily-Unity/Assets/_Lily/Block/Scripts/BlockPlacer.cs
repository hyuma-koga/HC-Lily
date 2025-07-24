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
            return false;
        }

        foreach (var tile in targetTiles)
        {
            Vector2Int from = tile - moveDir;

            if (!boardManager.IsTIleWithinBounds(tile))
            {
                if (!wallManager.IsSameColorWall(from, moveDir, block.blockColor))
                {
                    return false;
                }
            }
            else
            {
                //Note: 移動元からの方向に壁があるか
                if (wallManager.ExistsWall(from, moveDir) &&
                    !wallManager.IsSameColorWall(from, moveDir, block.blockColor))
                {
                    return false;
                }

                //Note: 移動先に逆方向の壁があるか
                if (wallManager.ExistsWall(tile, -moveDir) &&
                    !wallManager.IsSameColorWall(tile, -moveDir, block.blockColor))
                {
                    return false;
                }
            }
        }

        if (IsOverlapping(block, targetTiles))
        {
            return false;
        }

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