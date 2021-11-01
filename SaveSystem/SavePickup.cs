using UnityEngine;

public class SavePickup : SaveableObject
{
    [SerializeField] Vector2 spawnPos;

    void OnValidate()
    {
        spawnPos = transform.position;
    }

    public SavePickup()
    {
        objectType = PrefabType.Pickup;
    }

    public override SaveableObjectData CreateSaveData()
    {
        if(spawnPos==null)
        {
            Debug.LogError(name + " has no set spawn position!");
            return null;
        }

        PickupData pickupData = new PickupData(gameObject, spawnPos, PrefabId, objectType);

        return pickupData;
    }
}

[System.Serializable]
public class PickupData : SaveableObjectData
{
    public override void LoadAddinational(Transform parent, GameObject obj)
    {
        obj.transform.parent = parent;
    }

    public PickupData(GameObject obj, Vector2 worldPos, string id, PrefabType type) : base(obj, worldPos, id, type)
    {
    }
}