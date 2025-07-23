using UnityEngine;
using System.Collections.Generic;

public enum BlockShapeType
{
    Square,
    Rectangle,
    LShape,
    ZShape
}

public class BlockController : MonoBehaviour
{
    public BlockShapeType shapeType;
    public Vector2Int boardPosition;
    public Vector2Int size = Vector2Int.one;
    public Color blockColor;

    private static readonly Dictionary<BlockShapeType, List<Vector2Int>> shapeOffsets = new()
{
    // size 2x2
    { BlockShapeType.Square, new List<Vector2Int> { new(0, 0), new(1, 0), new(0, 1), new(1, 1) } },

    // size 2x1 ‰¡’·
    { BlockShapeType.Rectangle, new List<Vector2Int> { new(0, 0), new(1, 0) } },

    // LŽš (2x2)
    { BlockShapeType.LShape, new List<Vector2Int> { new(0,0), new(1,0), new(0,1) } },

    // ZŽš (3x2)
    { BlockShapeType.ZShape, new List<Vector2Int> { new(0,1), new(1,0), new(1,1), new(2, 0) } },
};

    public List<Vector2Int> GetOccupiedTiles(Vector2Int? testPos = null)
    {
        Vector2Int basePos = testPos ?? boardPosition;

        var offsets = shapeOffsets[shapeType];
        List<Vector2Int> tiles = new();

        foreach (var offset in offsets)
            tiles.Add(basePos + offset);

        return tiles;
    }

    public void MoveTo(Vector2Int newBoardPos)
    {
        boardPosition = newBoardPos;
        transform.position = CalculateWorldPosition(newBoardPos);
    }

    private Vector3 CalculateWorldPosition(Vector2Int boardPos)
    {
        Vector3 basePos = new Vector3(boardPos.x, transform.position.y, boardPos.y);
        Vector3 offset = shapeType switch
        {
            BlockShapeType.Square => new Vector3(0.5f, 0f, 0.5f),
            BlockShapeType.Rectangle => new Vector3(0.5f, 0f, 0f),
            BlockShapeType.LShape => new Vector3(0f, 0f, 0f),
            BlockShapeType.ZShape => new Vector3(1f, 0f, 0.5f),
            _ => Vector3.zero
        };
        return basePos + offset;
    }
}
