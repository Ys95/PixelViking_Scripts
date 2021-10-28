using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "itemsDatabase", menuName = "Resources Database/ItemObjectDatabase")]
public class ItemObjectDatabase : ScriptableObject
{
    [SerializeField] List<ItemObject> itemsList;

    Dictionary<ItemObject, string> getId = new Dictionary<ItemObject, string>();
    Dictionary<string, ItemObject> getItem = new Dictionary<string, ItemObject>();

    #region Getters

    public Dictionary<ItemObject, string> GetId { get => getId; }
    public Dictionary<string, ItemObject> GetItem { get => getItem; }

    #endregion Getters

    [ContextMenu("Refresh database")]
    public void RefreshDatabase()
    {
        ItemObject[] itemsArray = Resources.LoadAll<ItemObject>("Items");
        itemsList = new List<ItemObject>();
        itemsList.Clear();

        foreach (ItemObject item in itemsArray)
        {
            if (string.IsNullOrWhiteSpace(item.ItemId))
            {
                Debug.LogError("Item " + item.DisplayName + " has no id assigned!");
            }
            else
            {
                itemsList.Add(item);
            }
        }
    }

    public void RefreshDictionary()
    {
        RefreshDatabase();

        getId = new Dictionary<ItemObject, string>();
        getItem = new Dictionary<string, ItemObject>();

        foreach (ItemObject item in itemsList)
        {
            if (item == null || string.IsNullOrEmpty(item.ItemId)) continue;

            if (GetItem.ContainsKey(item.ItemId))
            {
                IdManager.DisplayDuplicatedIdError(GetItem[item.ItemId].name, item.name, item.ItemId);
                continue;
            }

            GetItem.Add(item.ItemId, item);
            GetId.Add(item, item.ItemId);
        }
    }
}