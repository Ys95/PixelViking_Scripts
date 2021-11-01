using UnityEngine;

public class SaveInteractable : SaveableObject
{
    public SaveInteractable()
    {
        objectType = PrefabType.Interactable;
    }

    public override SaveableObjectData CreateSaveData()
    {
        Vector2 worldPos = transform.position;
        bool wasInteractedWith = gameObject.GetComponent<IInteractable>().WasInteractedWith;
        InteractableData data = new InteractableData(gameObject, worldPos, PrefabId, objectType, wasInteractedWith);
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

    public InteractableData(GameObject obj, Vector2 worldPos, string id, PrefabType type, bool state) : base(obj, worldPos, id, type)
    {
        wasInteractedWith = state;
    }
}