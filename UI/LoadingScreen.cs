using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    #region Singleton

    static LoadingScreen instance = null;
    public static LoadingScreen Instance { get => instance; }

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

        DontDestroyOnLoad(this.gameObject);

        LevelManager.LoadingStarted += ShowLoadingScreen;
        LevelManager.LoadingFinished += HideLoadingScreen;
    }

    #endregion Singleton

    [SerializeField] GameObject loadingScreen;

    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
        GameManager.Pause(true);
    }

    public void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);
        GameManager.Pause(false);
    }

    void OnDestroy()
    {
        LevelManager.LoadingStarted -= ShowLoadingScreen;
        LevelManager.LoadingFinished -= HideLoadingScreen;
    }
}