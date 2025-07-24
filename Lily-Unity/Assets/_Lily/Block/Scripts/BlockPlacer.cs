using System.Collections.Generic;
using UnityEngine;

//Note: �u���b�N�̔z�u�ۂ�ݒu�������s��
public class BlockPlacer : MonoBehaviour
{
    private BoardManager boardManager;
    private WallManager wallManager;

    private void Awake()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        wallManager = FindFirstObjectByType<WallManager>();
    }

    //Note: �w��̃{�[�h���W�Ƀu���b�N��z�u�ł��邩�ǂ���
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
                //Note: �ړ�������̕����ɕǂ����邩
                if (wallManager.ExistsWall(from, moveDir) &&
                    !wallManager.IsSameColorWall(from, moveDir, block.blockColor))
                {
                    return false;
                }

                //Note: �ړ���ɋt�����̕ǂ����邩
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