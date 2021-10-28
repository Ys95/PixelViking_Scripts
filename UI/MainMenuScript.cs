using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] GameObject tutorialPopup;

    public void StartGameBtnClick() => tutorialPopup.SetActive(true);

    public void LoadGameBtnClick() => uiManager.OpenMenu(uiManager.LoadMenu);

    public void OptionsBtnClick() => uiManager.OpenMenu(uiManager.OptionsMenu);

    public void ExitBtnClick() => Application.Quit();

    public void GoBackBtnClick() => SceneManager.LoadScene(LevelManager.MAIN_MENU);

    public void TutorialPopupYesBtnClick() => SceneManager.LoadScene(LevelManager.TUTORIAL_LEVEL);

    public void TutorialPopupNoBtnClick() => SceneManager.LoadScene(LevelManager.LEVEL_1);

    void Update()
    {
        if (Cursor.visible == true) return;
        Cursor.visible = true;
    }
}