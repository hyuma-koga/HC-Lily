using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageClearChecker : MonoBehaviour
{
    private BoardManager boardManager;
    private bool hasCleared = false;

    [Header("ステージクリア判定対象の座標範囲")]
    public Vector2Int minCheckPos;
    public Vector2Int maxCheckPos;

    private void Awake()
    {
        enabled = false;
    }

    private void Start()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        StartCoroutine(EnableCheckerAfterDelay());
    }

    private IEnumerator EnableCheckerAfterDelay()
    {
        yield return null;
        yield return null; // 2フレーム待機で安全
        enabled = true;
    }

    private void Update()
    {
        if (hasCleared) return;

        if (IsStageCleared())
        {
            hasCleared = true;
            Debug.Log(" ステージクリア！");
            GameManager.Instance.OnGameClear();
        }
    }

    private bool IsStageCleared()
    {
        var blocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);

        foreach (var block in blocks)
        {
            var tiles = block.GetOccupiedTiles();

            foreach (var tile in tiles)
            {
                if (IsWithinCheckArea(tile))
                {
                    return false; // ブロックがまだチェック範囲内にある
                }
            }
        }

        return true; // どのブロックもチェック範囲に存在しない
    }

    private bool IsWithinCheckArea(Vector2Int tilePos)
    {
        return tilePos.x >= minCheckPos.x && tilePos.x <= maxCheckPos.x &&
               tilePos.y >= minCheckPos.y && tilePos.y <= maxCheckPos.y;
    }
}
