using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameUI;

    public void Show()
    {
        gameUI.SetActive(true);
    }

    public void Hide()
    {
        gameUI.SetActive(false);
    }
}