using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockRemover : MonoBehaviour
{
    private BoardManager boardManager;
    private WallManager wallManager;
    private StageClearChecker stageClearChecker;

    private void Start()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        wallManager = FindFirstObjectByType<WallManager>();
        stageClearChecker = FindFirstObjectByType<StageClearChecker>();
        StartCoroutine(CheckAndRemoveLoop());
    }

    private IEnumerator CheckAndRemoveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            var blocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);

            foreach (var block in blocks)
            {
                if (block == null || !block.enabled)
                    continue;

                if (IsCenterOverlappingSameColorWall(block))
                {
                    StartCoroutine(RemoveBlockWithAnimation(block));
                }
            }
        }
    }

    private bool IsCenterOverlappingSameColorWall(BlockController block)
    {
        Vector2Int blockCenter = block.boardPosition + block.size / 2;

        foreach (var wall in FindObjectsByType<WallComponent>(FindObjectsSortMode.None))
        {
            if (!wall.wallColor.Equals(block.blockColor)) continue;

            Vector2Int wallCenter = wall.boardPosition + wall.size / 2;

            if (blockCenter == wallCenter)
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator RemoveBlockWithAnimation(BlockController block)
    {
        block.enabled = false;

        if (block.blockRenderer == null)
        {
            Destroy(block.gameObject);
            yield break;
        }

        float duration = 0.3f;
        float elapsed = 0f;

        Material mat = block.blockRenderer.material;
        Color startColor = mat.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            mat.color = Color.Lerp(startColor, targetColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mat.color = targetColor;
        Destroy(block.gameObject);

        if (stageClearChecker != null)
        {
            stageClearChecker.CheckStageClear();
        }
    }
}