using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageClearChecker : MonoBehaviour
{
    private bool hasCleared = false;

    private void Start()
    {
        StartCoroutine(EnableCheckerAfterDelay());
    }

    private IEnumerator EnableCheckerAfterDelay()
    {
        yield return null;
        yield return null; // 2フレーム待機で安全
        CheckStageClear();
    }

    public void CheckStageClear()
    {
        if (hasCleared) return;

        if (IsStageCleared())
        {
            hasCleared = true;
            Debug.Log("ステージクリア！");
            GameManager.Instance.OnGameClear();
        }
    }

    private bool IsStageCleared()
    {
        // ブロックが1つも存在しないか？
        var blocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);
        return blocks.Length == 0;
    }
}