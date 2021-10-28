using UnityEngine;

public enum ConsumableEffect
{
    Healing,
    Regeneration,
    Damage,
    Armor,
}

public enum ConsumableType
{
    Food,
    Drink,
    Potion,
}

public abstract class ConsumableItem : ItemObject
{
    [SerializeField] protected ConsumableEffect effect;
    [SerializeField] protected ConsumableType consumableType;
    [SerializeField] protected float effectDuration;

    public ConsumableEffect Effect { get => effect; }
    public ConsumableType ConsumableType { get => consumableType; }
    public virtual float EffectDuration { get => effectDuration; }

    public void UseConsumable(ConsumableItemsBuffs buffSystem)
    {
        Debug.Log(effect.ToString() + " " + consumableType.ToString() + " used.");
        buffSystem.UseBuffItem(this);
    }

    public override void SetType()
    {
        type = ItemType.Consumable;
    }
}