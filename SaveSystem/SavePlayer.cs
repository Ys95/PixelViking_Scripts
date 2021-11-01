using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePlayer : SaveableObject
{
    [SerializeField] PlayerScriptReferences player;

    public override SaveableObjectData CreateSaveData()
    {
        int hp = player.Stats.Health;
        Vector2 worldPos = transform.position;

        InventorySaveData inventory = player.PlayerInventory.Inventory.CreateInventorySaveData();

        PlayerData data = new PlayerData(gameObject, worldPos, hp, SceneManager.GetActiveScene().buildIndex, inventory, objectType);

        return data;
    }

    public SavePlayer()
    {
        objectType = PrefabType.Player;
    }
}

[System.Serializable]
public class PlayerData : SaveableObjectData
{
    [SerializeField] int level;
    [SerializeField] int health;
    [SerializeField] InventorySaveData inventory;

    #region Getters

    public int Health { get => health; }
    public int Level { get => level; }
    public InventorySaveData Inventory { get => inventory; }

    #endregion Getters

    public override void LoadObject(Transform parent, PrefabsDatabase db)
    {
        LoadAddinational(null, null);
    }

    public override void LoadAddinational(Transform parents, GameObject obj)
    {
        Vector3 position = new Vector3(worldPosX, worldPosY, worldPosZ);

        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        player.transform.position = position;

        player.GetComponent<CharacterStats>().SetHealth(health);
        player.GetComponent<PlayerInventory>().Inventory.LoadInventory(inventory);
    }

    public PlayerData(GameObject obj, Vector2 worldPos, int hp, int lvl, InventorySaveData inv, PrefabType type) : base(obj, worldPos, "Player", type)
    {
        health = hp;
        level = lvl;
        inventory = inv;
    }
}