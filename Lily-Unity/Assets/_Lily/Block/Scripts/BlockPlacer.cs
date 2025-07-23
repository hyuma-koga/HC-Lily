using System.Collections.Generic;
using UnityEngine;

//Note: �u���b�N�̔z�u�ۂ�ݒu�������s��
public class BlockPlacer : MonoBehaviour
{
    private BoardManager boardManager;

    private void Awake()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
    }

    //Note: �w��̃{�[�h���W�Ƀu���b�N��z�u�ł��邩�ǂ���
    public bool CanPlace(BlockController block, Vector2Int targetPos, Vector2Int moveDir)
    {
        List<Vector2Int> targetTiles = block.GetOccupiedTiles(targetPos);

        //Note: �g���{�ǐF�`�F�b�N
        if (!boardManager.AreTilesInsideBoard(targetTiles, block.blockColor, moveDir))
        {
            return false;
        }

        //Note: ���u���b�N�Ƃ̏d�Ȃ�`�F�b�N
        if (boardManager.AreTilesOccupied(targetTiles, block))
        {
            return false;
        }

        return true;
    }
}