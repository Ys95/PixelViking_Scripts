using UnityEngine;

[CreateAssetMenu(fileName = "item_consumable_regenerationbuff_itemname", menuName = "Inventory System/Items/Consumable/RegenerationBuff")]
public class RegenerationBuffItem : ConsumableItem
{
    [SerializeField] int healedInTick;
    [SerializeField] int ticksAmount;
    [SerializeField] float timeBetweenTicks;

    public int HealedInTick { get => healedInTick; }
    public int TicksAmount { get => ticksAmount; }
    public float TimeBetweenTicks { get => timeBetweenTicks; }
    public override float EffectDuration { get => (float)ticksAmount * timeBetweenTicks; }

    public override void CreateAutoDescription()
    {
        autoDescription = "Regenerates " + healedInTick + "HP every " + timeBetweenTicks + " seconds for " + ((float)ticksAmount * timeBetweenTicks) + " seconds.";
    }

    public override void SetProperties()
    {
        effect = ConsumableEffect.Regeneration;
        effectDuration = (float)ticksAmount * timeBetweenTicks;
    }
}