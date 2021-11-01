using UnityEngine;

/// <summary>
/// Class for creating save data of objects and restoring it.
/// </summary>
public abstract class SaveableObject : MonoBehaviour
{
    [SerializeField] string prefabId;
    [SerializeField] protected PrefabType objectType;

    #region Getters

    public string PrefabId { get => prefabId; }
    public PrefabType ObjectType { get => objectType; }

    #endregion Getters

    [ContextMenu("Assign ID")]
    public void AssignId()
    {
        string id = IdManager.AssignId(prefabId);
        if (string.IsNullOrEmpty(id)) return;
        prefabId = id;
    } //won't run during gametime

    public abstract SaveableObjectData CreateSaveData();
}

[System.Serializable]
public abstract class SaveableObjectData
{
    [SerializeField] protected string prefabId;
    [SerializeField] protected PrefabType objectType;

    [SerializeField] protected float worldPosX;
    [SerializeField] protected float worldPosY;
    [SerializeField] protected float worldPosZ;

    #region Getters

    public Vector3 WorldPos { get => new Vector3(worldPosX, worldPosY, worldPosZ); }
    public string PrefabId { get => prefabId; }
    public PrefabType ObjectType { get => objectType; }

    #endregion Getters

    public virtual void LoadObject(Transform parent, PrefabsDatabase db)
    {
        GameObject prefab = db.GetPrefab(prefabId);
        Quaternion rotation = new Quaternion();
        GameObject loadedObject = GameObject.Instantiate(prefab, WorldPos, rotation);

        loadedObject.transform.position = WorldPos;

        LoadAddinational(parent, loadedObject);
    }

    public abstract void LoadAddinational(Transform parents, GameObject obj);

    public SaveableObjectData(GameObject obj, Vector2 worldPos, string id, PrefabType type)
    {
        objectType = type;
        worldPosX = worldPos.x;
        worldPosY = worldPos.y;
        prefabId = id;
    }
}