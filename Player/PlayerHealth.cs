using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] PlayerScriptReferences player;

    [Header("Components")]
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] CinemachineVirtualCamera cam;

    [Space]
    [SerializeField] Color colorWhenDamaged;
    [SerializeField] ParticleSystem damagedParticle;

    Color originalSpriteColor;

    public delegate void PlayerDied();

    public delegate void PlayerTookDamage();

    public static event PlayerDied PlayerIsDead;

    public static event PlayerTookDamage PlayerDamaged;

    void Awake()
    {
        originalSpriteColor = playerSprite.color;
    }

    public void HealDamage(int amount)
    {
        player.Stats.AddHealth(amount);
        player.Sounds.PlayerHealing.Play(transform.position);
        FloatingCombatText.Instance.SpawnHealedDamageText(amount, transform.position);
    }

    public void TakeDamage(int damage, Vector2 knockbackForce, Vector3 damagingObjectPosition)
    {
        if (!player.Status.CanBeDamaged || player.Status.IsDead) return;
        damagedParticle.Play();
        PlayerDamaged?.Invoke();

        damage -= player.Stats.DamageReduction;

        if (damage < 0)
        {
            damage = 0;
        }

        player.Stats.AddHealth(-damage);

        Knockback(damagingObjectPosition, knockbackForce);

        if (player.Stats.Health <= 0)
        {
            PlayerDeath();
            return;
        }

        StartCoroutine(IframeEffect());

        player.Sounds.PlayerHurt.Play(transform.position);
        FloatingCombatText.Instance.SpawnReceivedDamageText(damage, playerRB.position);
    }

    IEnumerator IframeEffect()
    {
        float timer = 1f;

        player.Status.CanBeDamaged = false;
        player.Status.CanAttack = false;
        gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");

        while (timer > 0f)
        {
            yield return new WaitForSeconds(0.2f);
            timer -= 0.2f;

            playerSprite.color = colorWhenDamaged;

            yield return new WaitForSeconds(0.2f);
            timer -= 0.2f;

            playerSprite.color = originalSpriteColor;
        }

        playerSprite.color = originalSpriteColor;
        player.Status.CanBeDamaged = true;
        player.Status.CanAttack = true;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void Knockback(Vector3 damagingObjectPosition, Vector2 knockbackForce)
    {
        if (player.Status.IsDead) return;

        float forceX = knockbackForce.x;
        float forceY = knockbackForce.y;

        if (damagingObjectPosition.x > playerRB.position.x)
        {
            forceX *= -1f;
        }

        knockbackForce = new Vector2(forceX, forceY);
        playerRB.AddForce(knockbackForce, ForceMode2D.Impulse);
    }

    void PlayerDeath()
    {
        player.Sounds.PlayerDeath.Play(transform.position);
        player.Stats.SetHealth(0);

        playerRB.constraints = RigidbodyConstraints2D.None;
        playerRB.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
        playerRB.AddTorque(100f);

        player.Status.IsDead = true;
        cam.Follow = null;

        player.Input.EnableInput(false);
        playerCollider.enabled = false;

        PlayerIsDead?.Invoke();
    }

    public float GetFacingDirection()
    {
        if (player.Status.IsFacingLeft) return -1f;
        return 1f;
    }
}