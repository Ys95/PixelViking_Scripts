using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles loading scenes
/// </summary>
public static class LevelManager
{
    #region Scenes

    public static readonly int MAIN_MENU = 1;
    public static readonly int TUTORIAL_LEVEL = 2;
    public static readonly int LEVEL_1 = 3;
    public static readonly int END_SCREEN = 4;

    #endregion Scenes

    static SaveData saveFile;
    static SaveData temporarySave; //save file that will be used to load game when player dies

    static int currentLevelId;
    static bool scenePrepared;

    static GameObject sceneLoaderDummy;

    public static SaveData TemporarySave { get => temporarySave; set => temporarySave = value; }
    public static int CurrentLevelId { get => currentLevelId; set => currentLevelId = value; }

    public delegate void LevelLoadingStarted();

    public delegate void LevelLoadingFinished();

    public static event LevelLoadingStarted LoadingStarted;

    public static event LevelLoadingFinished LoadingFinished;

    public static IEnumerator LoadAsynchronously(int index) //started from dummy gameobject
    {
        LoadingStarted?.Invoke();

        Debug.Log("Start loading level " + index);

        AsyncOperation loadingLevel = SceneManager.LoadSceneAsync(index);

        while (!loadingLevel.isDone && !scenePrepared)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.4f);
        LoadingFinished?.Invoke();

        GameObject.Destroy(sceneLoaderDummy);
    }

    static void LoadScene(int index)
    {
        GameManager.Pause(true);

        sceneLoaderDummy = new GameObject();
        sceneLoaderDummy.AddComponent<SceneLoader>();

        GameObject.Instantiate(sceneLoaderDummy);
        GameObject.DontDestroyOnLoad(sceneLoaderDummy);

        sceneLoaderDummy.GetComponent<SceneLoader>().Load(index);

        GameManager.Pause(false);
    }

    public static void LoadLevel(int level)
    {
        scenePrepared = true;
        temporarySave = null;
        LoadScene(level);
    }

    public static void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex;
        nextScene++;
        LoadScene(nextScene);
    }

    public static void ReloadLevel()
    {
        int id = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Reloading level");
        LoadLevel(id);
    }

    public static void LoadGame(SaveData saveFileArg)
    {
        saveFile = saveFileArg;
        temporarySave = saveFileArg;

        int level = saveFile.LevelId;
        currentLevelId = level;

        scenePrepared = false;
        SceneManager.sceneLoaded += PrepareScene;
        LoadScene(level);
    }

    public static void LoadTemporarySave()
    {
        int currentLevelId = SceneManager.GetActiveScene().buildIndex;

        if (temporarySave == null || temporarySave.IsEmpty || temporarySave.LevelId != currentLevelId)
        {
            ReloadLevel();
            return;
        }

        saveFile = temporarySave;
        Debug.Log("Loading temp save");
        SceneManager.sceneLoaded += PrepareScene;
        LoadScene(currentLevelId);
    }

    static void PrepareScene(Scene scene, LoadSceneMode mode)
    {
        GameManager.Pause(true);
        Databases.Instance.RefreshDictionaries();

        SaveableObjectsManager.Instance.WipeAllSaveableObjects();
        SaveableObjectsManager.Instance.LoadObjects(saveFile.GetSaveableObjectsList());

        SceneManager.sceneLoaded -= PrepareScene;
        scenePrepared = true;
        Debug.Log("Scene prepared");
        GameManager.Pause(false);
    }
}