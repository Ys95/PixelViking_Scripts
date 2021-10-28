using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to get save data or load it.
/// </summary>

public class SaveableObjectsManager : MonoBehaviour
{
    #region Singleton

    private static SaveableObjectsManager instance = null;
    public static SaveableObjectsManager Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion Singleton

    [SerializeField] private Transform pickups;
    [SerializeField] private Transform interactables;
    [SerializeField] private Transform destructibles;
    [SerializeField] private Transform enemies;

    private Transform GetTransform(PrefabType type)
    {
        if (type == PrefabType.Pickup) return pickups;
        if (type == PrefabType.Interactable) return interactables;
        if (type == PrefabType.Destructible) return destructibles;
        if (type == PrefabType.Enemy) return enemies;

        return null;
    }

    public void WipeAllSaveableObjects()
    {
        foreach (Transform child in transform)
        {
            Transform[] childrenOfChildren = new Transform[child.childCount];
            int i = 0;

            foreach (Transform childOfChild in child)
            {
                childrenOfChildren[i] = childOfChild;
                i++;
            }

            foreach (Transform childOfChild in childrenOfChildren)
            {
                GameObject.Destroy(childOfChild.gameObject);
            }
        }
    }

    public List<SaveableObjectData> GetSaveableObjectsData()
    {
        List<SaveableObjectData> list = new List<SaveableObjectData>();

        foreach (Transform child in transform)
        {
            foreach (Transform childOfChild in child)
            {
                SaveableObject saveable = childOfChild.GetComponent<SaveableObject>();

                if (saveable != null)
                {
                    SaveableObjectData data = saveable.CreateSaveData();
                    list.Add(data);
                }
                else
                {
                    Debug.LogWarning(childOfChild.name + " has no data to save!");
                }
            }
        }

        return list;
    }

    public void LoadObjects(List<SaveableObjectData> savedObjects)
    {
        foreach (SaveableObjectData savedObject in savedObjects)
        {
            Transform parent = GetTransform(savedObject.ObjectType);
            savedObject.LoadObject(parent, Databases.Instance.PrefabsDB);
        }
    }
}