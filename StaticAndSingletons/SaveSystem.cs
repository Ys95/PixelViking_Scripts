using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    #region Singleton

    private static SaveSystem instance = null;
    public static SaveSystem Instance { get => instance; }

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

        LoadSaveSlots();
    }

    #endregion Singleton

    [SerializeField] SavePlayer player;

    [Space]
    [SerializeField] string saveSlotPath;
    [SerializeField] string saveSlotExtension;

    [Space]
    [SerializeField] static SaveData[] saveSlots = new SaveData[3];
    [SerializeField] SaveData tempSave;

    public SaveData[] SaveSlots { get => saveSlots; }

    public SaveData CreateSaveData(string name)
    {
        SaveData saveData = new SaveData();

        List<SaveableObjectData> saveableObjectsData = SaveableObjectsManager.Instance.GetSaveableObjectsData();
        PlayerData playerData = (PlayerData)player.CreateSaveData();

        saveData.Create(SceneManager.GetActiveScene().buildIndex, name, playerData, saveableObjectsData);

        return saveData;
    }

    public void CreateSaveFile(int slot, string saveName)
    {
        saveSlots[slot] = CreateSaveData(saveName);
        LevelManager.TemporarySave = saveSlots[slot];

        string path = saveSlotPath + slot.ToString() + saveSlotExtension;

        string data = JsonUtility.ToJson(saveSlots[slot], true);
        File.WriteAllText(Application.persistentDataPath + path, data);
        Debug.Log("Created save file");
    }

    public void LoadGame(int slot)
    {
        if (saveSlots[slot] == null) return;
        if (saveSlots[slot].IsEmpty) return;
        LevelManager.LoadGame(saveSlots[slot]);
    }

    void LoadSaveSlots()
    {
        for (int i = 0; i < saveSlots.Length; i++)
        {
            string path = saveSlotPath + i.ToString() + saveSlotExtension;
            if (File.Exists(string.Concat(Application.persistentDataPath, path)))
            {
                string data = File.ReadAllText(string.Concat(Application.persistentDataPath, path));
                saveSlots[i] = JsonUtility.FromJson<SaveData>(data);
            }
        }
    }

    public void CreateTempSave()
    {
        tempSave = null;
        tempSave = CreateSaveData("tempSave");
        LevelManager.TemporarySave = tempSave;
    }

    public void LoadTempSave() => LevelManager.LoadTemporarySave();
}

[System.Serializable]
public class SaveData
{
    [SerializeField] bool isEmpty;
    [SerializeField] string fileName;
    [SerializeField] string fileCreationDate;

    [SerializeField] int levelId;
    [SerializeField] PlayerData playerData;
    [SerializeField] List<InteractableData> interactableData;
    [SerializeField] List<DestructibleObjectData> destructiblesData;
    [SerializeField] List<PickupData> pickupsData;
    [SerializeField] List<EnemyData> enemyData;

    public List<SaveableObjectData> GetSaveableObjectsList()
    {
        List<SaveableObjectData> list = new List<SaveableObjectData>(interactableData.Count + destructiblesData.Count + pickupsData.Count);

        list.AddRange(interactableData);
        list.AddRange(destructiblesData);
        list.AddRange(pickupsData);
        list.AddRange(enemyData);

        list.Add(playerData);

        return list;
    }

    #region Getters

    public int LevelId { get => levelId; }
    public PlayerData PlayerData { get => playerData; set => playerData = value; }
    public List<InteractableData> ChestsData { get => interactableData; }
    public List<DestructibleObjectData> DestructiblesData { get => destructiblesData; }
    public List<PickupData> PickupsData { get => pickupsData; }
    public List<EnemyData> EnemyData { get => enemyData; }
    public string FileName { get => fileName; }
    public string FileCreationDate { get => fileCreationDate; }
    public bool IsEmpty { get => isEmpty; }

    #endregion Getters

    public void Create(int lvl, string saveName, PlayerData playerDataArg, List<SaveableObjectData> saveableObjectsData)
    {
        levelId = lvl;
        fileName = saveName;
        fileCreationDate = DateTime.Now.ToString();

        playerData = playerDataArg;

        foreach (SaveableObjectData objectData in saveableObjectsData)
        {
            switch (objectData.ObjectType)
            {
                case PrefabType.Interactable:
                    {
                        interactableData.Add((InteractableData)objectData);
                        break;
                    }

                case PrefabType.Destructible:
                    {
                        destructiblesData.Add((DestructibleObjectData)objectData);
                        break;
                    }

                case PrefabType.Pickup:
                    {
                        pickupsData.Add((PickupData)objectData);
                        break;
                    }

                case PrefabType.Enemy:
                    {
                        enemyData.Add((EnemyData)objectData);
                        break;
                    }
            }
        }

        isEmpty = false;
    }

    public SaveData()
    {
        isEmpty = true;

        interactableData = new List<InteractableData>();
        destructiblesData = new List<DestructibleObjectData>();
        pickupsData = new List<PickupData>();
        enemyData = new List<EnemyData>();
    }
}