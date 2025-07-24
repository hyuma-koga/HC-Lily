using UnityEngine;
using System.Collections;

public class StageClearChecker : MonoBehaviour
{
    private BoardManager boardManager;
    private bool hasCleared = false;

    private void Start()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        StartCoroutine(EnableCheckerAfterDelay());
    }

    private IEnumerator EnableCheckerAfterDelay()
    {
        yield return null; 
        yield return null; // 必要なら2フレーム
        enabled = true;
    }

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        if (hasCleared) return;

        if (IsStageCleared())
        {
            hasCleared = true;
            GameManager.Instance.OnGameClear();
        }
    }

    private bool IsStageCleared()
    {
        var targetTiles = boardManager.GetAllBoardTilePositions();
        var blocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);

        foreach (var block in blocks)
        {
            var occupied = block.GetOccupiedTiles();

            foreach (var tile in occupied)
            {
                if (targetTiles.Contains(tile))
                {
                    return false;
                }
            }
        }

        return true;
    }
}