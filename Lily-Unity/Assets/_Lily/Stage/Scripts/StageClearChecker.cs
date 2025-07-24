using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageClearChecker : MonoBehaviour
{
    private BoardManager boardManager;
    private bool hasCleared = false;

    [Header("�X�e�[�W�N���A����Ώۂ̍��W�͈�")]
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
        yield return null; // 2�t���[���ҋ@�ň��S
        enabled = true;
    }

    private void Update()
    {
        if (hasCleared) return;

        if (IsStageCleared())
        {
            hasCleared = true;
            Debug.Log(" �X�e�[�W�N���A�I");
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
                    return false; // �u���b�N���܂��`�F�b�N�͈͓��ɂ���
                }
            }
        }

        return true; // �ǂ̃u���b�N���`�F�b�N�͈͂ɑ��݂��Ȃ�
    }

    private bool IsWithinCheckArea(Vector2Int tilePos)
    {
        return tilePos.x >= minCheckPos.x && tilePos.x <= maxCheckPos.x &&
               tilePos.y >= minCheckPos.y && tilePos.y <= maxCheckPos.y;
    }
}
