using UnityEngine;

public class SaveDestructibleObject : SaveableObject
{
    public SaveDestructibleObject()
    {
        objectType = PrefabType.Destructible;
    }

    public override SaveableObjectData CreateSaveData()
    {
        DestructibleObjectData data = new DestructibleObjectData(gameObject, PrefabId, objectType);
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

    public DestructibleObjectData(GameObject obj, string id, PrefabType type) : base(obj, id, type)
    {
    }
}