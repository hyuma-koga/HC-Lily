using System.Collections.Generic;
using UnityEngine;

// Note: �{�[�h�S�̂̊Ǘ��i�^�C���ʒu�E�ǁE���u���b�N�Ƃ̏d�Ȃ�Ȃǁj
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