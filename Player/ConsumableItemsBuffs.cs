using System.Collections;
using UnityEngine;

/// <summary>
/// Handles modifying character stats by items.
/// </summary>
public class ConsumableItemsBuffs : MonoBehaviour
{
    [SerializeField] PlayerScriptReferences player;

    [Space]
    [SerializeField] BuffSlotsDisplay buffUI;
    [SerializeField] BuffVisualEffects visualEffects;

    //stores buff coroutine that is currently active. null if none running
    IEnumerator foodEffectCoroutine;
    IEnumerator drinkEffectCoroutine;

    ConsumableItem activeFood;
    ConsumableItem activeDrink;

    public void RestoreHealth(int amount)
    {
        Debug.Log(this.name + " used");
        player.Health.HealDamage(amount);
    }

    public void UseBuffItem(ConsumableItem item)
    {
        IEnumerator buffEffectCoroutine;

        PlayVisualEffect(item.Effect);

        switch (item.Effect)
        {
            case ConsumableEffect.Armor:
                {
                    buffEffectCoroutine = BuffDefense((DefenseBuffItem)item);
                    player.Sounds.PlayerBuffUse.Play(transform.position);
                    break;
                }

            case ConsumableEffect.Damage:
                {
                    buffEffectCoroutine = BuffAttack((AttackBuffItem)item);
                    player.Sounds.PlayerBuffUse.Play(transform.position);
                    break;
                }

            case ConsumableEffect.Regeneration:
                {
                    buffEffectCoroutine = BuffRegeneration((RegenerationBuffItem)item);
                    player.Sounds.PlayerBuffUse.Play(transform.position);
                    break;
                }

            case ConsumableEffect.Healing:
                {
                    RestoreHealth(((HealingItem)item).AmountHealed);
                    player.Sounds.PlayerBuffUse.Play(transform.position);
                    return;
                }

            default:
                return;
        }

        ref ConsumableItem activeItemRef = ref activeFood;
        ref IEnumerator effectCoroutineRef = ref foodEffectCoroutine;

        //Find out which fields should be modified
        if (item.ConsumableType == ConsumableType.Drink)
        {
            activeItemRef = ref activeDrink;
            effectCoroutineRef = ref drinkEffectCoroutine;
        }

        if (activeItemRef != null)
        {
            ClearBuff(activeItemRef);
        }
        activeItemRef = item;
        effectCoroutineRef = buffEffectCoroutine;

        StartCoroutine(effectCoroutineRef);
        buffUI.UpdateDisplay(item.ConsumableType, item.Effect, item.EffectDuration);
    }

    void PlayVisualEffect(ConsumableEffect effect)
    {
        if (visualEffects == null) return;
        visualEffects.StartEffect(effect);
    }

    IEnumerator BuffAttack(AttackBuffItem item)
    {
        player.Stats.AddAttackPower(item.DamageBonus);
        yield return new WaitForSeconds(item.EffectDuration);

        ClearBuff(item);
    }

    IEnumerator BuffDefense(DefenseBuffItem item)
    {
        player.Stats.AddDamageReduction(item.DamageReduction);
        yield return new WaitForSeconds(item.EffectDuration);

        ClearBuff(item);
    }

    IEnumerator BuffRegeneration(RegenerationBuffItem item)
    {
        for (int i = 0; i < item.TicksAmount; i++)
        {
            yield return new WaitForSeconds(item.TimeBetweenTicks);
            player.Health.HealDamage(item.HealedInTick);
        }
        ClearBuff(item);
    }

    void ClearBuff(ConsumableItem itemArg)
    {
        switch (itemArg.Effect)
        {
            case ConsumableEffect.Damage:
                {
                    var dmgitem = (AttackBuffItem)itemArg;
                    player.Stats.AddAttackPower(-(dmgitem.DamageBonus));
                }
                break;

            case ConsumableEffect.Armor:
                {
                    var defitem = (DefenseBuffItem)itemArg;
                    player.Stats.AddDamageReduction(-defitem.DamageReduction);
                }
                break;

            case ConsumableEffect.Regeneration:
                break;
        }
        ClearBuffSlot(itemArg);
    }

    void ClearBuffSlot(ConsumableItem item)
    {
        if (item.ConsumableType == ConsumableType.Food)
        {
            activeFood = null;
            if (foodEffectCoroutine != null)
            {
                StopCoroutine(foodEffectCoroutine);
                foodEffectCoroutine = null;
            }
        }
        else if (item.ConsumableType == ConsumableType.Drink)
        {
            activeDrink = null;
            if (drinkEffectCoroutine != null)
            {
                StopCoroutine(drinkEffectCoroutine);
                drinkEffectCoroutine = null;
            }
        }
    }
}