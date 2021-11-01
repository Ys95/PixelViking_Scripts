using System.Collections.Generic;
using UnityEngine;

public enum PrefabType
{
    Enemy,
    Pickup,
    Player,
    Interactable,
    Destructible,
}

[CreateAssetMenu(fileName = "prefabsDatabase_name", menuName = "Resources Database/PrefabsDatabase")]
public class PrefabsDatabase : ScriptableObject
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject pickupTemplate;
    [SerializeField] GameObject lootTemplate;

    [SerializeField] List<PrefabInfo> pickupPrefabs;
    [SerializeField] List<PrefabInfo> enemyPrefabs;
    [SerializeField] List<PrefabInfo> interactablePrefabs;
    [SerializeField] List<PrefabInfo> destructiblesPrefabs;

    Dictionary<string, PrefabInfo> PrefabsDictionary;

    #region Getters

    public GameObject PlayerPrefab { get => playerPrefab; }
    public GameObject PickupTemplate { get => pickupTemplate; }
    public GameObject LootTemplate { get => lootTemplate; }

    #endregion Getters

    [System.Serializable]
    class PrefabInfo
    {
        [SerializeField] GameObject prefab;
        [SerializeField] string prefabName;
        [SerializeField] string prefabId;
        [SerializeField] PrefabType prefabType;

        public GameObject Prefab { get => prefab; }
        public string PrefabName { get => prefabName; }
        public string PrefabId { get => prefabId; }
        public PrefabType PrefabType { get => prefabType; }

        public PrefabInfo(GameObject prefabArg)
        {
            prefab = prefabArg;
        }

        public void UpdateInfo()
        {
            if (Prefab == null) return;
            SaveableObject info = Prefab.GetComponent<SaveableObject>();
            if (info == null) return;

            if (string.IsNullOrEmpty(info.PrefabId))
            {
                info.AssignId();
            }
            prefabId = info.PrefabId;
            prefabName = Prefab.name;
            prefabType = info.ObjectType;
        }
    }

    [ContextMenu("Update Database")]
    public void UpdateDatabase() //method intended to be called manually from the inspector
    {
        pickupPrefabs = new List<PrefabInfo>();
        enemyPrefabs = new List<PrefabInfo>();
        interactablePrefabs = new List<PrefabInfo>();
        destructiblesPrefabs = new List<PrefabInfo>();

        GameObject[] allPrefabs = Resources.LoadAll<GameObject>("Prefabs");

        foreach (GameObject prefab in allPrefabs)
        {
            PrefabInfo prefabInfo = new PrefabInfo(prefab);
            prefabInfo.UpdateInfo();

            //sort prefabs based on type
            switch (prefabInfo.PrefabType)
            {
                case PrefabType.Interactable:
                    {
                        interactablePrefabs.Add(prefabInfo);
                        break;
                    }

                case PrefabType.Pickup:
                    {
                        pickupPrefabs.Add(prefabInfo);
                        break;
                    }

                case PrefabType.Destructible:
                    {
                        destructiblesPrefabs.Add(prefabInfo);
                        break;
                    }

                case PrefabType.Enemy:
                    {
                        enemyPrefabs.Add(prefabInfo);
                        break;
                    }
            }
        }
    }

    [ContextMenu("RefreshDictionary")]
    public void RefreshDictionary()
    {
        List<PrefabInfo> allPrefabs = new List<PrefabInfo>(pickupPrefabs.Count + enemyPrefabs.Count + interactablePrefabs.Count + destructiblesPrefabs.Count);

        allPrefabs.AddRange(pickupPrefabs);
        allPrefabs.AddRange(enemyPrefabs);
        allPrefabs.AddRange(interactablePrefabs);
        allPrefabs.AddRange(destructiblesPrefabs);

        PrefabsDictionary = new Dictionary<string, PrefabInfo>();

        foreach (PrefabInfo prefab in allPrefabs)
        {
            if (PrefabsDictionary.ContainsKey(prefab.PrefabId))
            {
                IdManager.DisplayDuplicatedIdError(PrefabsDictionary[prefab.PrefabId].PrefabName, prefab.PrefabName, prefab.PrefabId);
            }
            else
            {
                PrefabsDictionary.Add(prefab.PrefabId, prefab);
            }
        }
    }

    public GameObject GetPrefab(string id)
    {
        if (PrefabsDictionary.ContainsKey(id))
        {
            return PrefabsDictionary[id].Prefab;
        }
        else if (id == "Player")
        {
            return playerPrefab;
        }
        else
        {
            Debug.Log("Key " + id + " not found");
            return null;
        }
    }
}