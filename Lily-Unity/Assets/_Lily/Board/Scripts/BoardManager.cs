using System.Collections.Generic;
using UnityEngine;

//Note: ボード全体の管理（タイル位置・壁・他ブロックとの重なりなど）
public class BoardManager : MonoBehaviour
{
    [SerializeField] private List<WallComponent> wallComponents = new();

    private HashSet<Vector2Int> validTiles = new();
    private int minX, maxX, minY, maxY;

    private void Awake()
    {
        InitializeBoardBounds();
    }

    private void InitializeBoardBounds()
    {
        TileComponent[] tiles = FindObjectsByType<TileComponent>(FindObjectsSortMode.None);

        minX = int.MaxValue;
        maxX = int.MinValue;
        minY = int.MaxValue;
        maxY = int.MinValue;

        foreach (var tile in tiles)
        {
            Vector2Int pos = tile.boardPosition;
            validTiles.Add(pos);

            minX = Mathf.Min(minX, pos.x);
            maxX = Mathf.Max(maxX, pos.x);
            minY = Mathf.Min(minY, pos.y);
            maxY = Mathf.Max(maxY, pos.y);
        }

        if (wallComponents == null || wallComponents.Count == 0)
        {
            wallComponents = new List<WallComponent>(FindObjectsByType<WallComponent>(FindObjectsSortMode.None));
        }
    }

    //Note: 指定タイルがボード内に存在するか
    public bool IsTileWithinBounds(Vector2Int pos)
    {
        return validTiles.Contains(pos);
    }

    //Note: 枠外移動が許されるか（壁の色を使った制限含む）
    public bool AreTilesInsideBoard(List<Vector2Int> tiles, Color blockColor, Vector2Int moveDir)
    {
        foreach (var tile in tiles)
        {
            if (!IsTileWithinBounds(tile))
            {
                //Note: 枠外に出ている → 同じ色の壁があるかチェック
                Vector2Int from = tile - moveDir;

                if (!IsWallSameColor(from, moveDir, blockColor))
                {
                    return false;
                }
            }
        }

        return true;
    }

    //Note: 他ブロックと重なっているか
    public bool AreTilesOccupied(List<Vector2Int> tiles, BlockController self)
    {
        BlockController[] allBlocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);

        foreach (var block in allBlocks)
        {
            if (block == self)
            {
                continue;
            }

            //Note: 相手ブロックの占有マスは現在のboardPositionに基づく
            List<Vector2Int> otherTiles = block.GetOccupiedTiles();

            foreach (var tile in tiles)
            {
                if (otherTiles.Contains(tile))
                {
                    return true;
                }
            }
        }

        return false;
    }

    //Note: 指定位置に指定方向の同色の壁があるか
    public bool IsWallSameColor(Vector2Int pos, Vector2Int dir, Color color)
    {
        foreach (var wall in wallComponents)
        {
            if (wall.boardPosition == pos &&
                wall.direction == GetDirectionFromVector(dir) &&
                wall.wallColor == color)
            {
                return true;
            }
        }
        return false;
    }

    private WallDirection GetDirectionFromVector(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return WallDirection.Up;
        if (dir == Vector2Int.down) return WallDirection.Down;
        if (dir == Vector2Int.left) return WallDirection.Left;
        if (dir == Vector2Int.right) return WallDirection.Right;

        Debug.LogWarning($"不正な方向: {dir}");
        return WallDirection.Up;
    }
}
