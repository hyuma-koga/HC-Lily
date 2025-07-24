using UnityEngine;

//Note: À•W•ÏŠ·‚Æ•ûŒüŒvŽZ
public static class BoardCoordinateHelper
{
    public static Vector2Int WorldToBoard(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x),
            Mathf.FloorToInt(worldPosition.z)
        );
    }

    public static Vector3 BoardToWorld(Vector2Int boardPosition, BlockShapeType shapeType, float y = 0f)
    {
        Vector3 basePos = new Vector3(boardPosition.x, y, boardPosition.y);

        Vector3 offset = shapeType switch
        {
            BlockShapeType.Square => new Vector3(0.5f, 0f, 0.5f),
            BlockShapeType.Rectangle => new Vector3(0.5f, 0f, 0f),
            BlockShapeType.LShape => Vector3.zero,
            BlockShapeType.ZShape => new Vector3(1f, 0f, 0.5f),
            _ => Vector3.zero
        };

        return basePos + offset;
    }

    public static WallDirection GetDirectionFromVector(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
        {
            return WallDirection.Up;
        }

        if (dir == Vector2Int.down)
        {
            return WallDirection.Down;
        }

        if (dir == Vector2Int.left)
        {
            return WallDirection.Left;
        }

        if (dir == Vector2Int.right)
        {
            return WallDirection.Right;
        }

        return WallDirection.Up;
    }
}