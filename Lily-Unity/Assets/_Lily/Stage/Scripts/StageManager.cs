using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject[] stagePrefabs;
    private GameObject  currentStageInstance;
    private int         currentStageIndex = 0;

    private void SpawnStage(int index)
    {
        if (index < 0 || index >= stagePrefabs.Length)
        {
            return;
        }

        GameObject stagePrefab = stagePrefabs[index];
        currentStageInstance = Instantiate(stagePrefab);

        var boardManager = FindFirstObjectByType<BoardManager>();

        if (boardManager != null)
        {
            boardManager.InitializeBoardBounds();
        }

        var wallManager = FindFirstObjectByType<WallManager>();
        if (wallManager != null)
        {
            wallManager.ReloadWalls();
        }

        foreach (var block in currentStageInstance.GetComponentsInChildren<BlockController>())
        {
            block.InitializePosition(); // ← Start() ではなくこのように別メソッドで初期化するのが安全
        }
    }

    public void LoadFirstStage()
    {
        currentStageIndex = 0;
        SpawnStage(currentStageIndex);
    }

    public void LoadNextStage()
    {
        currentStageIndex++;
        
        if (currentStageIndex >= stagePrefabs.Length)
        {
            return;
        }

        Destroy(currentStageInstance);
        SpawnStage(currentStageIndex);
    }

    public void DestroyCurrentStage()
    {
        if (currentStageInstance != null)
        {
            Destroy(currentStageInstance);
        }
    }
}