using UnityEngine;

[CreateAssetMenu(fileName = "item_consumable_defensebuff_itemname", menuName = "Inventory System/Items/Consumable/DefenseBuff")]
public class DefenseBuffItem : ConsumableItem
{
    [SerializeField] int damageReduction;

    public int DamageReduction { get => damageReduction; }

    public override void CreateAutoDescription()
    {
        autoDescription = "Reduces damage taken by " + damageReduction + " for " + effectDuration + " seconds. ";
    }

    public override void SetProperties()
    {
        effect = ConsumableEffect.Armor;
    }
}