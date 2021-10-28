using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    [Header("Components")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] BoxCollider2D projectileCollider;
    [SerializeField] Rigidbody2D rb;

    [Header("Effects")]
    [SerializeField] ParticleSystem onHitParticle;
    [SerializeField] SoundEffect onSpawnSound;
    [SerializeField] SoundEffect onHitSound;

    [Space]
    [SerializeField] float speed;
    [SerializeField] float maxVelocity;
    [SerializeField] float lifeTime;
    [SerializeField] int damage;
    [SerializeField] Vector2 knockbackStrength;
    [SerializeField] bool canBeDeflected;
    [SerializeField] bool canBeDestroyed;

    Vector3 targetDir;
    bool facingLeft;
    bool isMoving;
    bool isPlayerProjectile;

    #region Get/Set

    public float Speed { get => speed; set => speed = value; }
    public float LifeTime { get => lifeTime; set => lifeTime = value; }
    public int Damage { get => damage; set => damage = value; }

    #endregion Get/Set

    public void StartMoving(Transform target)
    {
        targetDir = target.position - transform.position;

        if (targetDir.x < 0f)
        {
            facingLeft = true;
        }

        targetDir = Vector3.Normalize(targetDir);

        Flip();
        isMoving = true;
        SoundManager.Instance.PlaySound(SoundCategory.Enemy, onSpawnSound, transform);

        StartCoroutine(ProjectileLifeTime());
    }

    public void StartMoving(float direction)
    {
        targetDir.x = direction;

        if (targetDir.x < 0f)
        {
            facingLeft = true;
        }

        targetDir = Vector3.Normalize(targetDir);

        Flip();
        isMoving = true;
        SoundManager.Instance.PlaySound(SoundCategory.Enemy, onSpawnSound, transform);

        StartCoroutine(ProjectileLifeTime());
    }

    public void IsPlayerProjectile(bool playerProjectile)
    {
        if (playerProjectile)
        {
            this.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
            isPlayerProjectile = true;
            return;
        }
        this.gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        isPlayerProjectile = false;
    }

    IEnumerator ProjectileLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        ProjectileHit();
    }

    void Flip()
    {
        if (facingLeft)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    public void TakeDamage(int damage, Vector2 knockbackForce, Vector3 damagingObjectPosition)
    {
        if (canBeDeflected) Deflect();
        if (canBeDestroyed) ProjectileHit();
    }

    void Deflect()
    {
        targetDir *= -1f;
        rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        IsPlayerProjectile(true);
        facingLeft = !facingLeft;
        Flip();
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        Vector2 force = new Vector2(targetDir.x * speed, 0f);
        rb.AddForce(force);
        ClampVelocity();
    }

    void ClampVelocity()
    {
        Vector2 clampedVelocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
        rb.velocity = clampedVelocity;
    }

    void ProjectileHit()
    {
        SoundManager.Instance.PlaySound(SoundCategory.Enemy, onHitSound, transform);

        if (onHitParticle != null)
        {
            OnHitEffect();
        }
    }

    void OnHitEffect()
    {
        projectileCollider.enabled = false;
        sprite.enabled = false;
        isMoving = false;

        onHitParticle.Play();
        Invoke("DisableGameobject", onHitParticle.main.duration);
    }

    void DisableGameobject()
    {
        Debug.Log("disable projectile");
        transform.gameObject.SetActive(false); //return projectile to the pool
    }

    void OnDisable()
    {
        isMoving = false;
        facingLeft = false;

        projectileCollider.enabled = true;
        sprite.enabled = true;

        IsPlayerProjectile(false);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void OnValidate()
    {
        if (canBeDeflected) canBeDestroyed = false;
        if (canBeDestroyed) canBeDeflected = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable health = collision.collider.GetComponent<IDamageable>();

        if (health != null)
        {
            health.TakeDamage(damage, knockbackStrength, transform.position);
            if (isPlayerProjectile) FloatingCombatText.Instance.SpawnDealedDamageText(damage, transform.position);
        }

        ProjectileHit();
    }

    public void Knockback(Vector3 damagingObjectPosition, Vector2 knockbackForce)
    {
        //not used
    }

    public float GetFacingDirection()
    {
        if (facingLeft) return -1f;
        return 1f;
    }
}