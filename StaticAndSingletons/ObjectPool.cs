using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    #region Singleton

    private static ObjectPool instance = null;
    public static ObjectPool Instance { get => instance; }

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

        ActivatePools();
    }

    #endregion Singleton

    [SerializeField] List<PooledObject> pooledObjects;

    Dictionary<string, PooledObject> pooledObjectByTag;

    [System.Serializable]
    class PooledObject
    {
        string tag;
        [SerializeField] GameObject prefab;
        [SerializeField] int amountInPool;
        [SerializeField] bool expandable;

        [HideInInspector]
        List<GameObject> objectsList;

        public string Tag { get => tag; }

        public void Activate()
        {
            tag = prefab.tag;
            objectsList = new List<GameObject>();

            for (int i = 0; i < amountInPool; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectsList.Add(obj);
            }
        }

        public GameObject GetGameObject()
        {
            for (int i = 0; i < objectsList.Count; i++)
            {
                if (!objectsList[i].activeInHierarchy) return objectsList[i];
            }

            if (expandable)
            {
                Debug.LogWarning("Pool " + tag + " expanded.");

                GameObject addObject = Instantiate(prefab);
                addObject.SetActive(false);
                objectsList.Add(addObject);
                return addObject;
            }

            Debug.LogError("Not enough objects in " + tag + " pool");
            return null;
        }
    }

    void ActivatePools()
    {
        pooledObjectByTag = new Dictionary<string, PooledObject>();

        foreach (PooledObject obj in pooledObjects)
        {
            obj.Activate();
            pooledObjectByTag.Add(obj.Tag, obj);
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        return pooledObjectByTag[tag].GetGameObject();
    }
}