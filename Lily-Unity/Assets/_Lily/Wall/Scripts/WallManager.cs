using UnityEngine;
using System.Collections.Generic;

//Note: 壁のデータの管理と色比較・存在の確認
public class WallManager : MonoBehaviour
{
    private List<WallComponent> wallComponents = new();

    private void Awake()
    {
        wallComponents.AddRange(FindObjectsByType<WallComponent>(FindObjectsSortMode.None));
    }

    public bool ExistsWall(Vector2 pos, Vector2Int dir)
    {
        WallDirection wallDir = BoardCoordinateHelper.GetDirectionFromVector(dir);

        foreach (var wall in wallComponents)
        {
            if (wall.boardPosition == pos && wall.direction == wallDir)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsSameColorWall(Vector2Int pos, Vector2Int dir, Color color)
    {
        WallDirection wallDir = BoardCoordinateHelper.GetDirectionFromVector(dir);

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

    private bool IsColorApproximately(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
                Mathf.Abs(a.g - b.g) < tolerance &&
                Mathf.Abs(a.b - b.b) < tolerance;
    }
}
