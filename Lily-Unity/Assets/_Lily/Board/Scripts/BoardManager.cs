using System.Collections.Generic;
using UnityEngine;

// Note: ボード全体の管理（タイル位置・壁・他ブロックとの重なりなど）
public class BoardManager : MonoBehaviour
{
    private HashSet<Vector2Int> validTiles = new();
    public int                  Width => width;
    public int                  Height => height;
    private int                 width;
    private int                 height;

    // ステージ生成後に呼び出すことで、タイルの座標範囲を記録する
    public void InitializeBoardBounds()
    {
        validTiles.Clear();

        TileComponent[] tiles = FindObjectsByType<TileComponent>(FindObjectsSortMode.None);

        int maxX = 0;
        int maxY = 0;

        foreach (var tile in tiles)
        {
            validTiles.Add(tile.boardPosition);

            if (tile.boardPosition.x > maxX) maxX = tile.boardPosition.x;
            if (tile.boardPosition.y > maxY) maxY = tile.boardPosition.y;
        }

        width = maxX + 1;
        height = maxY + 1;
    }

    public bool IsTileWithinBounds(Vector2Int pos)
    {
        return validTiles.Contains(pos);
    }

    public List<Vector2Int> GetAllBoardTilePositions()
    {
        return new List<Vector2Int>(validTiles);
    }
}