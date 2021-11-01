using UnityEngine;

public class SaveDestructibleObject : SaveableObject
{
    public SaveDestructibleObject()
    {
        objectType = PrefabType.Destructible;
    }

    public override SaveableObjectData CreateSaveData()
    {
        Vector2 worldPos = transform.position;
        DestructibleObjectData data = new DestructibleObjectData(gameObject, worldPos, PrefabId, objectType);
        return data;
    }
}

[System.Serializable]
public class DestructibleObjectData : SaveableObjectData
{
    public override void LoadAddinational(Transform parent, GameObject obj)
    {
        obj.transform.parent = parent;
    }

    public DestructibleObjectData(GameObject obj, Vector2 worldPos, string id, PrefabType type) : base(obj, worldPos, id, type)
    {
    }
}