using System.Collections.Generic;
using UnityEngine;

// Note: ボード全体の管理（タイル位置・壁・他ブロックとの重なりなど）
public class BoardManager : MonoBehaviour
{
    private HashSet<Vector2Int> validTiles = new();

    private void Awake()
    {
        InitializeBoardBounds();
    }

    private void InitializeBoardBounds()
    {
        TileComponent[] tiles = FindObjectsByType<TileComponent>(FindObjectsSortMode.None);

        foreach (var tile in tiles)
        {
            validTiles.Add(tile.boardPosition);
        }
    }

    public bool IsTIleWithinBounds(Vector2Int pos)
    {
        return validTiles.Contains(pos);
    }
}