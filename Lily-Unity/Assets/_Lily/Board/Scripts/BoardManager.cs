using System.Collections.Generic;
using UnityEngine;

// Note: ボード全体の管理（タイル位置・壁・他ブロックとの重なりなど）
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

    // Note: 指定タイルがボード内に存在するか
    public bool IsTileWithinBounds(Vector2Int pos)
    {
        return validTiles.Contains(pos);
    }

    // 色問わず壁の有無だけチェック
    private bool IsWallExists(Vector2Int pos, Vector2Int dir)
    {
        WallDirection wallDir = GetDirectionFromVector(dir);

        foreach (var wall in wallComponents)
        {
            if (wall.boardPosition == pos && wall.direction == wallDir)
            {
                return true;
            }
        }
        return false;
    }


    // Note: 枠外移動が許されるか（壁の色を使った制限含む）
    public bool AreTilesInsideBoard(List<Vector2Int> tiles, Color blockColor, Vector2Int moveDir)
    {
        foreach (var tile in tiles)
        {
            if (!IsTileWithinBounds(tile))
            {
                Vector2Int from = tile - moveDir;

                // 色が一致していない or 壁自体が無い場合はfalse
                if (!IsWallSameColor(from, moveDir, blockColor))
                {
                    return false;
                }
            }
            else
            {
                // ボード内であっても、その方向に壁があり、色が一致しないなら通過禁止
                Vector2Int from = tile - moveDir;
                if (IsWallExists(from, moveDir) && !IsWallSameColor(from, moveDir, blockColor))
                {
                    return false;
                }
            }
        }

        return true;
    }

    // Note: 他ブロックと重なっているか
    public bool AreTilesOccupied(List<Vector2Int> tiles, BlockController self)
    {
        BlockController[] allBlocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);

        foreach (var block in allBlocks)
        {
            if (block == self) continue;

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

    //  色を誤差許容で比較する
    private bool IsColorApproximately(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }

    // Note: 指定位置に指定方向の同色の壁があるか
    public bool IsWallSameColor(Vector2Int pos, Vector2Int dir, Color color)
    {
        WallDirection wallDir = GetDirectionFromVector(dir);

        foreach (var wall in wallComponents)
        {
            if (wall.boardPosition == pos &&
                wall.direction == wallDir &&
                IsColorApproximately(wall.wallColor, color))
            {
                return true;
            }
        }
        return false;
    }

    // Note: Vector2Int方向をWallDirectionに変換
    private WallDirection GetDirectionFromVector(Vector2Int dir)
    {
        if (dir == Vector2Int.up) return WallDirection.Up;
        if (dir == Vector2Int.down) return WallDirection.Down;
        if (dir == Vector2Int.left) return WallDirection.Left;
        if (dir == Vector2Int.right) return WallDirection.Right;

        return WallDirection.Up;
    }
}