using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// Note: 壁のデータの管理と色比較・存在の確認
public class WallManager : MonoBehaviour
{
    private List<WallComponent> wallComponents = new();

    private void Awake()
    {
        wallComponents.AddRange(FindObjectsByType<WallComponent>(FindObjectsSortMode.None));
    }

    public bool ExistsWall(Vector2Int pos, Vector2Int dir)
    {
        WallDirection wallDir = BoardCoordinateHelper.GetDirectionFromVector(dir);

        foreach (var wall in wallComponents)
        {
            if (!wall.directions.Contains(wallDir))
            {
                continue;
            }

            if (wall.GetOccupiedPositions().Contains(pos))
            {
                return true;
            }
        }

        return false;
    }

    public bool IsBlocked(Vector2Int pos, Color blockColor)
    {
        foreach (var wall in wallComponents)
        {
            if (wall.GetOccupiedPositions().Contains(pos) &&
                !IsColorApproximately(wall.wallColor, blockColor))
            {
                return true; // 異色の壁がある → 通行不可
            }
        }
        return false; // 同色 or 壁なし → 通行可
    }

    public bool IsSameColorWall(Vector2Int pos, Vector2Int dir, Color color)
    {
        WallDirection wallDir = BoardCoordinateHelper.GetDirectionFromVector(dir);

        foreach (var wall in wallComponents)
        {
            if (!wall.directions.Contains(wallDir))
            {
                continue;
            }

            if (wall.GetOccupiedPositions().Contains(pos) &&
                IsColorApproximately(wall.wallColor, color))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsColorApproximately(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }
}
