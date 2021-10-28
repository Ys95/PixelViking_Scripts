using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    [Header("Components")]
    [SerializeField] CanvasGroup canvasGroup;

    void OnEnable()
    {
        GameManager.Pause(true);
    }

    void OnDisable()
    {
        GameManager.Pause(false);
    }

    public void SaveBtnClick() => uiManager.OpenMenu(uiManager.SaveMenu);

    public void LoadBtnClick() => uiManager.OpenMenu(uiManager.LoadMenu);

    public void OptionsBtnClick() => uiManager.OpenMenu(uiManager.OptionsMenu);

    public void ResumeBtnClick() => uiManager.CloseMenu(uiManager.PauseMenu);

    public void ExitBtnClick() => uiManager.GoToMainMenu();
}