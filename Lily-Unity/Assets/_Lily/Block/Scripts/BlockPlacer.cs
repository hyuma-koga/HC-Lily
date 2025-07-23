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

        foreach (var tile in targetTiles)
        {
            if (!boardManager.IsTIleWithinBounds(tile))
            {
                Vector2Int from = tile - moveDir;
                
                if (!wallManager.IsSameColorWall(from, moveDir, block.blockColor))
                {
                    Debug.LogWarning($" �g�O + �F�s��v or �ǂȂ�: {tile} �� {from} dir:{moveDir}");
                    return false;
                }
            }
            else
            {
                Vector2Int from = tile - moveDir;

                if (wallManager.ExistsWall(from, moveDir) && !wallManager.IsSameColorWall(from, moveDir, block.blockColor))
                {
                    Debug.LogWarning($" �g�������ǕǐF�s��v: {tile} �� {from} dir:{moveDir}");
                    return false;
                }
            }
        }

        if (IsOverlapping(block, targetTiles))
        {
            Debug.LogWarning("���u���b�N�Əd�Ȃ��Ă���");
            return false;
        }

        Debug.Log(" �z�u�\�iCanPlace �ʉ߁j");
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