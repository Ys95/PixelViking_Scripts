using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] Rigidbody2D enemyRB;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator animator;

    [Space]
    [SerializeField] CharacterStats stats;
    [SerializeField] CharacterStatus status;
    [SerializeField] EnemyBehaviour enemyBehaviour;
    [SerializeField] EnemyAttack enemyAttack;
    [SerializeField] LootDropper lootDropper;

    [Space]
    [SerializeField] Color colorWhenDamaged;
    [SerializeField] ParticleSystem deathParticles;

    public delegate void OnGettingHit();

    public event OnGettingHit GotHit;

    Color originalSpriteColor;

    bool isDead;

    bool isStunned; //status effect during which enemy is unable to perform any action

    void Awake()
    {
        originalSpriteColor = sprite.color;
    }

    public void TakeDamage(int damage, Vector2 knockbackForce, Vector3 damagingObjectPosition)
    {
        damage -= stats.DamageReduction;
        GotHit?.Invoke();
        animator.SetTrigger("hurt");
        stats.AddHealth(-damage);
        Knockback(damagingObjectPosition, knockbackForce);

        if (stats.Health <= 0) EnemyDeath();
        StartCoroutine(Stun(0.2f));

        StopCoroutine(GotHitEffect());
        StartCoroutine(GotHitEffect());
    }

    public void Knockback(Vector3 damagingObjectPosition, Vector2 knockbackForce)
    {
        if (isDead) return;

        if (damagingObjectPosition.x < enemyRB.position.x)
        {
            enemyRB.AddRelativeForce(knockbackForce * enemyRB.mass, ForceMode2D.Impulse);
        }
        else
        {
            enemyRB.AddRelativeForce(new Vector2(knockbackForce.x * -1 * enemyRB.mass, knockbackForce.y), ForceMode2D.Impulse);
        }
    }

    IEnumerator Stun(float duration) //disable enemy actions after he gets hit
    {
        if (isStunned) yield return 0;

        isStunned = true;

        if (enemyAttack != null) enemyAttack.AttackEnabled(false);
        enemyBehaviour.MovementEnabled(false);

        yield return new WaitForSeconds(duration);

        enemyBehaviour.MovementEnabled(true);

        if (enemyAttack != null) enemyAttack.AttackEnabled(true);

        isStunned = false;

        yield return 0;
    }

    IEnumerator GotHitEffect()
    {
        sprite.color = colorWhenDamaged;
        yield return new WaitForSeconds(0.2f);
        sprite.color = originalSpriteColor;
    }

    void EnemyDeath() //called when enemy health reaches 0.
    {
        lootDropper.DropLoot();
        SoundManager.Instance.PlaySound(SoundCategory.Enemy, SoundEffect.enemy_Death, transform);

        if (deathParticles != null)
        {
            deathParticles.transform.parent = null;
            deathParticles.Play();
            Destroy(this.transform.parent.gameObject);
        }
    }

    void DestroyEnemy() => Destroy(this.transform.parent.gameObject);

    public float GetFacingDirection()
    {
        if (status.IsFacingLeft) return -1f;
        return 1f;
    }
}