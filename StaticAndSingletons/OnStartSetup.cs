using UnityEngine;
using UnityEngine.SceneManagement;

public class OnStartSetup : MonoBehaviour
{
    #region Singleton

    private static OnStartSetup instance = null;
    public static OnStartSetup Instance { get => instance; }

    #endregion Singleton

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        SetUp();
    }

    void SetUp()
    {
        PlayerSettings.SetVSyncOption();

        SceneManager.LoadScene(LevelManager.MAIN_MENU);
    }
}