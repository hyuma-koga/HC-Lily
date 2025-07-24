using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TitleUI titleUI;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameClearUI gameClearUI;
    [SerializeField] private StageManager stageManager;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        ShowTitleUI();
    }

    public void StartGame()
    {
        titleUI.Hide();
        gameUI.Show();
        stageManager.LoadFirstStage();
    }

    public void OnGameClear()
    {
        gameUI.Hide();
        gameClearUI.Show();
    }

    public void OnNextStage()
    {
        gameClearUI.Hide();
        stageManager.LoadNextStage();
        gameUI.Show();
    }

    private void ShowTitleUI()
    {
        titleUI.Show();
        gameUI.Hide();
        gameClearUI.Hide();
    }
}
