using UnityEngine;

/// <summary>
/// Handles communication between InventoryObject and character.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] PlayerScriptReferences player;

    [Space]
    [SerializeField] InventoryObject inventory;

    [Space]
    [SerializeField] GameObject throwedItemPrefab;
    [SerializeField] Vector2 itemThrowForce;

    [Space]
    [SerializeField] Transform castedSpellSpawn;
    [SerializeField] ParticleSystem castedSpellParticles;

    public InventoryObject Inventory { get => inventory; }

    void Awake()
    {
        inventory.Activate();
        inventory.Database.RefreshDictionary();
    }

    public void UseItem(ItemObject item)
    {
        if (!inventory.HasItem(item)) return;

        if (item is ConsumableItem) UseConsumable((ConsumableItem)item);
        if (item is SpellItem) UseSpell((SpellItem)item);

        ((ConsumableItem)item).UseConsumable(player.ItemBuffs);
        inventory.RemoveItem(item, 1);
    }

    public void UseItem(int slotId)
    {
        ItemObject item = inventory.InventorySlotsList[slotId].Item;
        if (item == null) return;

        if (item is ConsumableItem) UseConsumable((ConsumableItem)item);
        if (item is SpellItem) UseSpell((SpellItem)item);
    }

    void UseConsumable(ConsumableItem consumable)
    {
        consumable.UseConsumable(player.ItemBuffs);
        inventory.RemoveItem(consumable, 1);
    }

    void UseSpell(SpellItem spell)
    {
        spell.CastSpell(castedSpellSpawn, transform.localScale.x);
        inventory.RemoveItem(spell, 1);
        StartCoroutine(spell.SpawnParticles(castedSpellSpawn));
    }

    public void PickUpItem(ItemObject item)
    {
        inventory.AddItem(item, 1);
    }

    public void ThrowItem(ItemObject item, int amount) //TODO: object pooling
    {
        inventory.RemoveAllItems(item);
        for (int i = 0; i < amount; i++)
        {
            GameObject throwedItem = Instantiate(throwedItemPrefab, transform);

            float directionX;

            if (player.Status.IsFacingLeft)
            {
                directionX = -1f;
                Debug.Log("Left throw");
            }
            else
            {
                directionX = 1f;
                Debug.Log("Roght throw");
            }

            float addinationalForce = (float)i; //addination x force will be added for every item throwed to separate them
            Vector2 force = new Vector2((itemThrowForce.x + addinationalForce) * directionX, itemThrowForce.y);

            throwedItem.GetComponent<PickupScript>().PlayerThrowedItem(item, force);
        }
    }

    void OnApplicationQuit()
    {
        inventory.WipeInventory();
    }
}