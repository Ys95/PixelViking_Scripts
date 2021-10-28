using UnityEngine;

[CreateAssetMenu(fileName = "item_consumable_attackbuff_itemname", menuName = "Inventory System/Items/Consumable/AttackBuff")]
public class AttackBuffItem : ConsumableItem
{
    [SerializeField] int damageBonus;

    public int DamageBonus { get => damageBonus; }

    public override void SetProperties()
    {
        effect = ConsumableEffect.Damage;
    }

    public override void CreateAutoDescription()
    {
        autoDescription = "Increased attack by " + damageBonus + " for " + effectDuration + " seconds.";
    }
}