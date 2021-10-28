using UnityEngine;

public class SavePickup : SaveableObject
{
    public SavePickup()
    {
        objectType = PrefabType.Pickup;
    }

    public override SaveableObjectData CreateSaveData()
    {
        Vector3 pos = this.gameObject.transform.position;
        PickupData pickupData = new PickupData(gameObject, PrefabId, objectType);

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

    public PickupData(GameObject obj, string id, PrefabType type) : base(obj, id, type)
    {
    }
}