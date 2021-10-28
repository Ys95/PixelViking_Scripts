using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterStatus status;
    [SerializeField] CharacterStats stats;
    [SerializeField] Collider2D enemyCollider;

    bool canAttack = true;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != Tags.PLAYER) return;

        IDamageable damageableObject = collision.collider.GetComponent<IDamageable>();
        if (damageableObject == null || !canAttack) return;

        damageableObject.TakeDamage(stats.AttackPower, stats.KnockbackStrength, enemyCollider.transform.position);
        StartCoroutine(PauseMovementCoroutine());
    }

    IEnumerator PauseMovementCoroutine()
    {
        status.CanMove = false;
        yield return new WaitForSeconds(0.5f);
        status.CanMove = true;
    }

    public void AttackEnabled(bool attack)
    {
        canAttack = attack;
    }
}