using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerScriptReferences player;

    [Space]
    [Header("Components")]
    [SerializeField] Transform attackPoint;

    [Space]
    [SerializeField] int usedStamina;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask targetLayers;

    bool IsAttackPossible()
    {
        //Attack will trigger if:
        //1. Player has enough stamina OR filled his combo bar
        //2. Player is not alredy attacking
        //3. Player doesnt have his attack disabled

        if (!player.Status.CanAttack) return false;
        if (player.Status.IsAttacking) return false;
        if (!player.Stamina.HasEnoughStamina(usedStamina))
        {
            if (!player.Combo.IsComboReady) return false;
        }

        return true;
    }

    public void PrepareAttack()
    {
        if (!IsAttackPossible()) return;

        player.Status.IsAttacking = true;

        if (player.Combo.IsComboReady)
        {
            PrepareComboAttack();
            return;
        }

        player.Animations.TriggerAttack();
        player.Sounds.PlayerAttack.Play(transform.position);
        player.Stamina.UseStamina(usedStamina);
    }

    void PrepareComboAttack()
    {
        player.Animations.TriggerComboAttack();
        player.Combo.UseCombo();
        player.Stamina.RestoreStamina(1);
        PerformAttack(true);

        player.Sounds.PlayerComboAttack.Play(transform.position);
    }

    /// <summary>
    /// If attack is not a combo this will be only called by animation event.
    /// </summary>
    public void PerformAttack(bool isCombo)
    {
        float range = attackRange;
        bool hitEnemy = false;
        if (isCombo) range *= 2f;

        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(attackPoint.position, range, targetLayers);

        foreach (Collider2D target in targetsHit)
        {
            Debug.Log("Hit: " + target.name);
            IDamageable damageableTarget = target.GetComponent<IDamageable>();
            if (damageableTarget == null) return;

            if (target.tag == Tags.ENEMY) hitEnemy = true;

            damageableTarget.TakeDamage(player.Stats.AttackPower, player.Stats.KnockbackStrength, transform.position);

            int damageFloatingText = player.Stats.AttackPower;
            player.Sounds.PlayerAttackHit.Play(transform.position);

            if (isCombo)
            {
                Vector2 comboKnockback = new Vector2(0f, 6f);

                damageableTarget.TakeDamage(player.Stats.AttackPower, comboKnockback, transform.position);
                damageFloatingText += player.Stats.AttackPower;

                player.Sounds.PlayerAttackHit.Play(transform.position);
                player.Status.IsAttacking = false;
                Debug.Log("COMBO");
            }
            if (hitEnemy) FloatingCombatText.Instance.SpawnDealedDamageText(damageFloatingText, target.transform.position);
        }
        if (hitEnemy && !isCombo) player.Combo.StartComboMeter();
        player.Status.IsAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}