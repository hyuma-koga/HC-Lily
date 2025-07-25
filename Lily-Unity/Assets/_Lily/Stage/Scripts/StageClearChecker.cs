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
        yield return null; // 2�t���[���ҋ@�ň��S
        CheckStageClear();
    }

    public void CheckStageClear()
    {
        if (hasCleared) return;

        if (IsStageCleared())
        {
            hasCleared = true;
            Debug.Log("�X�e�[�W�N���A�I");
            GameManager.Instance.OnGameClear();
        }
    }

    private bool IsStageCleared()
    {
        // �u���b�N��1�����݂��Ȃ����H
        var blocks = FindObjectsByType<BlockController>(FindObjectsSortMode.None);
        return blocks.Length == 0;
    }
}