using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    [SerializeField] ItemObjectDatabase database;
    [SerializeField] int maxInventorySlots = 16;
    [SerializeField] int occupiedSlots;
    [SerializeField] int gold;

    [SerializeField] InventorySlot[] inventorySlotsList;
    Dictionary<string, int> ownedItems = new Dictionary<string, int>();

    #region Getters

    public int Gold { get => gold; }
    public InventorySlot[] InventorySlotsList { get => inventorySlotsList; }
    public Dictionary<string, int> OwnedItems { get => ownedItems; }
    public int MaxInventorySlots { get => maxInventorySlots; }
    public int OccupiedSlots { get => occupiedSlots; }
    public ItemObjectDatabase Database { get => database; }

    #endregion Getters

    public void Activate()
    {
        inventorySlotsList = new InventorySlot[MaxInventorySlots];
        WipeInventory();

        Debug.Log("inventory acitvated");
    }

    void UpdateOccupiedSlots()
    {
        occupiedSlots = 0;
        foreach (InventorySlot slot in inventorySlotsList)
        {
            if (slot.Item != null) occupiedSlots++;
        }
    }

    public bool HasItem(ItemObject item)
    {
        if (OwnedItems.ContainsKey(item.ItemId)) return true;
        else return false;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public void AddItem(ItemObject item, int amount)
    {
        int inventorySlotId;

        if (item == null)
        {
            Debug.LogWarning("Picked up null item!");
            return;
        }

        if (item.Type == ItemType.GoldCoin)
        {
            CoinObject coin = (CoinObject)item;
            AddGold(coin.Amount * amount);
            return;
        }

        if (HasItem(item))
        {
            inventorySlotId = OwnedItems[item.ItemId];
            InventorySlotsList[inventorySlotId].ItemsAmount += amount;
            Debug.Log("Player has item " + item.DisplayName + " , adding +" + amount);
        }
        else
        {
            bool foundEmptySlot = false;
            for (int i = 0; i < MaxInventorySlots; i++)
            {
                if (InventorySlotsList[i].Item == null)
                {
                    InventorySlotsList[i] = new InventorySlot(database.GetItem[item.ItemId], amount, i);
                    OwnedItems.Add(item.ItemId, i);
                    Debug.Log("Item " + item.DisplayName + " +" + amount);
                    foundEmptySlot = true;
                }
                if (foundEmptySlot) break;
            }
            if (!foundEmptySlot) Debug.Log("Inventory is full");
        }
        UpdateOccupiedSlots();
    }

    public void SwapSlots(int originalSlotId, int destinationSlotId)
    {
        InventorySlot originalSlot = InventorySlotsList[originalSlotId];
        InventorySlot destinationSlot = InventorySlotsList[destinationSlotId];

        InventorySlot temp = new InventorySlot(originalSlot.Item, originalSlot.ItemsAmount, originalSlot.SlotId);

        if (InventorySlotsList[destinationSlotId].Item == null) //if destination slot is empty, move item there
        {
            OwnedItems[originalSlot.Item.ItemId] = destinationSlotId; //update slot id in dictionary

            InventorySlotsList[destinationSlotId] = temp; //move item to new slot
            InventorySlotsList[originalSlotId].WipeSlot();
        }
        else //if not, swap items
        {
            OwnedItems[originalSlot.Item.ItemId] = destinationSlotId; //update slots id in dictionary
            OwnedItems[destinationSlot.Item.ItemId] = originalSlotId;

            InventorySlotsList[originalSlotId] = InventorySlotsList[destinationSlotId]; //swap items slots
            InventorySlotsList[destinationSlotId] = temp;
        }
    }

    public void RemoveItem(ItemObject item, int amount)
    {
        if (!OwnedItems.ContainsKey(item.ItemId)) return;

        int slotId = OwnedItems[item.ItemId];
        InventorySlotsList[slotId].RemoveAmount(amount);

        if (InventorySlotsList[slotId].ItemsAmount <= 0)
        {
            OwnedItems.Remove(item.ItemId);
            InventorySlotsList[slotId].Item = null;
        }

        UpdateOccupiedSlots();
    }

    public void RemoveAllItems(ItemObject item)
    {
        if (!OwnedItems.ContainsKey(item.ItemId)) return;

        int slotId = OwnedItems[item.ItemId];
        InventorySlotsList[slotId].WipeSlot();
        OwnedItems.Remove(item.ItemId);

        UpdateOccupiedSlots();
    }

    public void WipeInventory()
    {
        for (int i = 0; i < InventorySlotsList.Length; i++)
        {
            InventorySlotsList[i] = new InventorySlot(null, 0, i);
        }
        ownedItems = new Dictionary<string, int>();
        occupiedSlots = 0;
        gold = 0;
    }

    public InventorySaveData CreateInventorySaveData()
    {
        int inventorySize = InventorySlotsList.Length;
        string[] itemsId = new string[inventorySize];
        int[] itemsAmounts = new int[inventorySize];

        for (int i = 0; i < InventorySlotsList.Length; i++)
        {
            if (InventorySlotsList[i].Item != null)
            {
                itemsId[i] = (InventorySlotsList[i].Item.ItemId);
                itemsAmounts[i] = (InventorySlotsList[i].ItemsAmount);
            }
            else
            {
                itemsId[i] = (string.Empty);
                itemsAmounts[i] = 0;
            }
        }

        InventorySaveData data = new InventorySaveData(itemsId, itemsAmounts, Gold, inventorySize);
        return data;
    }

    public void LoadInventory(InventorySaveData save)
    {
        WipeInventory();
        OwnedItems.Clear();

        for (int i = 0; i < InventorySlotsList.Length; i++)
        {
            string id = save.ItemsId[i];

            if (!string.IsNullOrEmpty(id))
            {
                ItemObject item = database.GetItem[id];
                Debug.Log("loaded item: " + item.DisplayName + " in slot " + i);
                int amount = save.ItemsAmounts[i];

                InventorySlotsList[i].Item = item;
                InventorySlotsList[i].ItemsAmount = amount;

                OwnedItems.Add(id, i);
            }
            else
            {
                Debug.Log("Slot " + i + " empty.");
                InventorySlotsList[i].Item = null;
                InventorySlotsList[i].ItemsAmount = 0;
            }
        }
        gold = save.Gold;

        Debug.Log("inventory loaded");
    }

    [System.Serializable]
    public class InventorySlot
    {
        [SerializeField] ItemObject item;
        [SerializeField] int itemsAmount;
        [SerializeField] int slotId;

        #region Get/Set

        public ItemObject Item { get => item; set => item = value; }
        public int ItemsAmount { get => itemsAmount; set => itemsAmount = value; }
        public int SlotId { get => slotId; }

        #endregion Get/Set

        public InventorySlot(ItemObject itemArg, int amount, int id)
        {
            item = itemArg;
            itemsAmount = amount;
            slotId = id;
        }

        public void AddAmount(int amount)
        {
            itemsAmount += amount;
        }

        public void RemoveAmount(int amount)
        {
            itemsAmount -= amount;
            if (ItemsAmount <= 0)
            {
                WipeSlot();
            }
        }

        public void WipeSlot()
        {
            item = null;
            itemsAmount = 0;
        }
    }
}

[System.Serializable]
public struct InventorySaveData
{
    [SerializeField] string[] itemsId;
    [SerializeField] int[] itemsAmounts;
    [SerializeField] private int gold;
    [SerializeField] int inventorySize;

    #region Getters

    public string[] ItemsId { get => itemsId; }
    public int[] ItemsAmounts { get => itemsAmounts; }
    public int Gold { get => gold; }

    #endregion Getters

    public InventorySaveData(string[] itemsIdArg, int[] itemsAmountsArg, int goldArg, int size)
    {
        itemsId = itemsIdArg;
        itemsAmounts = itemsAmountsArg;
        gold = goldArg;
        inventorySize = size;
    }
}