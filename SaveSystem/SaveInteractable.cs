using UnityEngine;

public class SaveInteractable : SaveableObject
{
    public SaveInteractable()
    {
        objectType = PrefabType.Interactable;
    }

    public override SaveableObjectData CreateSaveData()
    {
        bool wasInteractedWith = gameObject.GetComponent<IInteractable>().WasInteractedWith;
        InteractableData data = new InteractableData(gameObject, PrefabId, objectType, wasInteractedWith);
        return data;
    }
}

[System.Serializable]
public class InteractableData : SaveableObjectData
{
    [SerializeField] private bool wasInteractedWith;

    public bool WasInteractedWith { get => wasInteractedWith; }

    public override void LoadAddinational(Transform parent, GameObject obj)
    {
        obj.transform.parent = parent;

        var interactable = obj.GetComponent<IInteractable>();

        interactable.LoadState(wasInteractedWith);
    }

    public InteractableData(GameObject obj, string id, PrefabType type, bool state) : base(obj, id, type)
    {
        wasInteractedWith = state;
    }
}