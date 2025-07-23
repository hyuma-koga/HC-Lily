using System.Collections.Generic;
using UnityEngine;

//Note: ブロックの配置可否や設置処理を行う
public class BlockPlacer : MonoBehaviour
{
    private BoardManager boardManager;

    private void Awake()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
    }

    //Note: 指定のボード座標にブロックを配置できるかどうか
    public bool CanPlace(BlockController block, Vector2Int targetPos, Vector2Int moveDir)
    {
        List<Vector2Int> targetTiles = block.GetOccupiedTiles(targetPos);

        //Note: 枠内＋壁色チェック
        if (!boardManager.AreTilesInsideBoard(targetTiles, block.blockColor, moveDir))
        {
            return false;
        }

        //Note: 他ブロックとの重なりチェック
        if (boardManager.AreTilesOccupied(targetTiles, block))
        {
            return false;
        }

        return true;
    }
}