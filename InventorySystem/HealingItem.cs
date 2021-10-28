using UnityEngine;

[CreateAssetMenu(fileName = "item_consumable_healing_itemname", menuName = "Inventory System/Items/Consumable/Healing")]
public class HealingItem : ConsumableItem
{
    [SerializeField] int amountHealed;

    public int AmountHealed { get => amountHealed; }
    public override float EffectDuration { get => 0f; }

    public override void SetProperties()
    {
        effect = ConsumableEffect.Healing;

        effectDuration = 0f;
    }

    public override void CreateAutoDescription()
    {
        autoDescription = "Instantly heals " + amountHealed + " HP.";
    }
}