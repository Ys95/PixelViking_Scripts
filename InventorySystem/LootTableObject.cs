using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "lootTable_name", menuName = "Inventory System/Loot Table")]
public class LootTableObject : ScriptableObject
{
    [SerializeField] int weightsSum;

    [Space]
    [SerializeField] int dropsAmount;
    [SerializeField] List<Loot> LootList;

    [Space]
    [SerializeField] ItemObject guarantedLoot;
    [SerializeField] int amountOfGuarantedItems;

    [Space]
    [SerializeField] [TextArea(5, 10)] string dropsSimulation;

    [System.Serializable]
    class Loot
    {
        [SerializeField] ItemObject item;
        [SerializeField] int dropChance;

        #region Get/Set

        public ItemObject Item { get => item; }
        public int DropChance { get => dropChance; set => dropChance = value; }

        #endregion Get/Set

        public Loot(ItemObject itemArg)
        {
            item = itemArg;
            dropChance = 0;
        }
    }

    void OnValidate()
    {
        weightsSum = 0;
        foreach (Loot loot in LootList)
        {
            weightsSum += loot.DropChance;
        }
    }

    public ItemObject RollLoot() //used to determine what items enemy will drop
    {
        int roll = Random.Range(1, weightsSum + 1);

        foreach (Loot loot in LootList)
        {
            if (roll <= loot.DropChance) return loot.Item;
            roll -= loot.DropChance;
        }
        return null;
    }

    public ItemObject[] CreateLoot()
    {
        List<ItemObject> droppedItems = new List<ItemObject>();

        for (int i = 0; i < dropsAmount; i++)
        {
            ItemObject rolledItem = RollLoot();
            if (rolledItem == null) continue;
            droppedItems.Add(rolledItem);
        }

        for (int i = 0; i < amountOfGuarantedItems; i++)
        {
            if (guarantedLoot == null) continue;
            droppedItems.Add(guarantedLoot);
        }

        ItemObject[] droppedItemsArray = droppedItems.ToArray();

        return droppedItemsArray;
    }

    [ContextMenu("Simulate drops")]
    void TestDropChance()
    {
        Dictionary<ItemObject, int> dropAmounts = new Dictionary<ItemObject, int>();
        ItemObject[] drops = CreateLoot();

        foreach (ItemObject drop in drops)
        {
            if (dropAmounts.ContainsKey(drop))
            {
                dropAmounts[drop]++;
            }
            else
            {
                dropAmounts.Add(drop, 1);
            }
        }

        dropsSimulation = "";
        foreach (KeyValuePair<ItemObject, int> drop in dropAmounts)
        {
            string dropName;

            if (drop.Key != null)
            {
                dropName = drop.Key.DisplayName;
            }
            else
            {
                dropName = "nothing";
            }

            string temp = "Dropped item: " + dropName + " || amount: " + drop.Value + "\n";
            dropsSimulation += temp;
        }
    }
}