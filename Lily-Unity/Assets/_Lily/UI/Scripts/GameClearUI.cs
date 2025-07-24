using UnityEngine;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] private GameObject gameClearUI;

    public void Show()
    {
        gameClearUI.SetActive(true);
    }

    public void Hide()
    {
        gameClearUI.SetActive(false);
    }

    public void OnNextStageButton()
    {
        FindFirstObjectByType<StageManager>()?.LoadNextStage();
        Hide();
    }
}