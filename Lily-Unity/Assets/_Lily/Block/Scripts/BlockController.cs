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
    public Renderer blockRenderer; // BlockVisual を参照

    private BoardManager boardManager;

    private static readonly Dictionary<BlockShapeType, List<Vector2Int>> shapeOffsets = new()
    {
        { BlockShapeType.Square,    new List<Vector2Int> { new(0, 0), new(1, 0), new(0, 1), new(1, 1) } },
        { BlockShapeType.Rectangle, new List<Vector2Int> { new(0, 0), new(1, 0) } },
        { BlockShapeType.LShape,    new List<Vector2Int> { new(0, 0), new(1, 0), new(0, 1) } },
        { BlockShapeType.ZShape,    new List<Vector2Int> { new(0, 1), new(1, 0), new(1, 1), new(2, 0) } },
    };

    private void Awake()
    {
        if (blockRenderer == null)
        {
            blockRenderer = GetComponentInChildren<Renderer>();
        }
    }

    private void Start()
    {
        if (blockRenderer != null)
        {
            blockColor = blockRenderer.material.color;
        }
    }

    public void InitializePosition()
    {
        boardManager = FindFirstObjectByType<BoardManager>();

        if (boardManager == null)
        {
            return;
        }

        boardPosition = BoardCoordinateHelper.WorldToBoard(transform.position);
    }

    //Note: ブロックがボード上のどのタイルを占有しているか
    public List<Vector2Int> GetOccupiedTiles(Vector2Int? testPos = null)
    {
        Vector2Int basePos = testPos ?? boardPosition;
        List<Vector2Int> tiles = new();

        foreach (var offset in shapeOffsets[shapeType])
        {
            tiles.Add(basePos + offset);
        }
        return tiles;
    }

    public void MoveTo(Vector2Int newBoardPos)
    {
        boardPosition = newBoardPos;
        transform.position = BoardCoordinateHelper.BoardToWorld(newBoardPos, shapeType, transform.position.y);
    }

    public bool IsInsideBoard(BoardManager board)
    {
        var tiles = GetOccupiedTiles();

        foreach (var tile in tiles)
        {
            if (board.IsTileWithinBounds(tile))
            {
                return true;
            }
        }
        return false;
    }
}