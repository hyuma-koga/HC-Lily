using UnityEngine;
using System.Collections.Generic;

public enum WallDirection
{
    Up,
    Down,
    Left,
    Right
}

[ExecuteAlways]
public class WallComponent : MonoBehaviour
{
    public Vector2Int boardPosition;
    public WallDirection[] directions;
    public Color wallColor;
    public Vector2Int size = new Vector2Int(1, 1);

    // この壁が占有するすべてのタイル座標を返す
    public List<Vector2Int> GetOccupiedPositions()
    {
        List<Vector2Int> positions = new();

        for (int dx = 0; dx < size.x; dx++)
        {
            for (int dy = 0; dy < size.y; dy++)
            {
                positions.Add(new Vector2Int(boardPosition.x + dx, boardPosition.y + dy));
            }
        }

        return positions;
    }

    private void Start()
    {
        UpdateWallColor();
    }

    private void OnValidate()
    {
        UpdateWallColor();
    }

    private void UpdateWallColor()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            wallColor = renderer.sharedMaterial?.color ?? Color.white;
        }
    }
}