using System.Collections.Generic;
using UnityEngine;

//Note: �{�[�h�S�̂̊Ǘ��i�^�C���ʒu�E�ǁE���u���b�N�Ƃ̏d�Ȃ�Ȃǁj
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

    //Note: �w��^�C�����{�[�h���ɑ��݂��邩
    public bool IsTileWithinBounds(Vector2Int pos)
    {
        return validTiles.Contains(pos);
    }

    //Note: �g�O�ړ���������邩�i�ǂ̐F���g���������܂ށj
    public bool AreTilesInsideBoard(List<Vector2Int> tiles, Color blockColor, Vector2Int moveDir)
    {
        foreach (var tile in tiles)
        {
            if (!IsTileWithinBounds(tile))
            {
                //Note: �g�O�ɏo�Ă��� �� �����F�̕ǂ����邩�`�F�b�N
                Vector2Int from = tile - moveDir;

                if (!IsWallSameColor(from, moveDir, blockColor))
                {
                    return false;
                }
            }
        }

        return true;
    }

    //Note: ���u���b�N�Əd�Ȃ��Ă��邩
    public bool AreTilesOccupied(List<Vector2Int> tiles, BlockController self)
    {
        BlockController[] allBlocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);

        foreach (var block in allBlocks)
        {
            if (block == self)
            {
                continue;
            }

            //Note: ����u���b�N�̐�L�}�X�͌��݂�boardPosition�Ɋ�Â�
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

    //Note: �w��ʒu�Ɏw������̓��F�̕ǂ����邩
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

        Debug.LogWarning($"�s���ȕ���: {dir}");
        return WallDirection.Up;
    }
}
