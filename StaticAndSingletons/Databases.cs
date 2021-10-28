using UnityEngine;

public class Databases : MonoBehaviour
{
    #region Singleton

    private static Databases instance = null;
    public static Databases Instance { get => instance; }

    private void Awake()
    {
        isReady = false;

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        RefreshDictionaries();

        isReady = true;
    }

    #endregion Singleton

    bool isReady;

    [SerializeField] PrefabsDatabase prefabsDB;
    [SerializeField] ItemObjectDatabase itemsDB;
    [SerializeField] AudioAssetsDatabase audioDB;

    public void RefreshDictionaries()
    {
        if (prefabsDB != null) prefabsDB.RefreshDictionary();
        if (itemsDB != null) itemsDB.RefreshDictionary();
        if (audioDB != null) audioDB.RefreshDictionary();
    }

    public PrefabsDatabase PrefabsDB { get => prefabsDB; }
    public ItemObjectDatabase ItemsDB { get => itemsDB; }
    public AudioAssetsDatabase AudioDB { get => audioDB; }
    public bool IsReady { get => isReady; }
}