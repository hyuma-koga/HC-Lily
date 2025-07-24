using UnityEngine;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private GameObject titleUI;

    public void Show()
    {
        titleUI.SetActive(true);
    }

    public void Hide()
    {
        titleUI.SetActive(false);
    }

    public void OnStartButton()
    {
        GameManager.Instance.StartGame();
    }
}