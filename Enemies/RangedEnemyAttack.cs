using System.Collections;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] BoxCollider2D enemyCollider;
    [SerializeField] Rigidbody2D enemyRb;

    [Space]
    [SerializeField] CharacterStatus status;
    [SerializeField] EnemyBehaviour behaviour;
    [SerializeField] EnemyHealth health;

    [Space]
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] Transform[] eyes;

    [Header("Effects")]
    [SerializeField] ParticleSystem onAttackParticle;
    [SerializeField] Material enemyMaterial;
    [SerializeField] SpriteGlowEffect idleGlow;
    [SerializeField] SpriteGlowEffect onAttackGlow;

    [Space]
    [SerializeField] LayerMask targetsLayer;
    [SerializeField] float viewRange;
    [SerializeField] float afterAttackCooldown;
    [SerializeField] float beforeAttackDelay;
    [SerializeField] Vector2 attackRecoil;

    bool canAttack = true;
    bool targetSpotted;
    bool inCameraViewRange;

    Vector2 currentTargetDirection;

    BoxCollider2D projectileCollider;

    [System.Serializable]
    struct SpriteGlowEffect
    {
        public Color GlowColor;
        public float GlowIntensinity;

        public Color Effect { get => GlowColor * GlowIntensinity; }
    }

#if UNITY_EDITOR

    void OnValidate() => UnityEditor.EditorApplication.delayCall += _OnValidate;

    void _OnValidate()
    {
        UnityEditor.EditorApplication.delayCall -= _OnValidate;
        if (this == null) return;
        if (enemyMaterial == null) return;
        enemyMaterial.color = idleGlow.Effect;
    }

#endif

    void Awake()
    {
        health.GotHit += InterruptAttacking;
        projectileCollider = enemyProjectile.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        InvokeRepeating(nameof(SearchForTarget), 0f, 0.1f);
    }

    void FlipSprite(float direction)
    {
        Vector3 scale = transform.localScale;

        if (direction < 0f)
        {
            scale.x = -1;
            transform.localScale = scale;
            status.IsFacingLeft = true;
        }
        else if (direction > 0f)
        {
            scale.x = 1;
            transform.localScale = scale;
            status.IsFacingLeft = false;
        }
    }

    void SearchForTarget()
    {
        Vector2 direction;

        if (status.IsFacingLeft) direction = Vector2.left;
        else direction = Vector2.right;

        for (int i = 0; i < eyes.Length; i++)
        {
            Vector2 raycastBehindStartPoint = new Vector2((eyes[i].position.x - (enemyCollider.bounds.extents.x * 2) * direction.x), eyes[i].position.y);

            Vector2 boxCastSize = new Vector2(viewRange, projectileCollider.bounds.size.y);

            /*            RaycastHit2D boxCastFrontHit = Physics2D.BoxCast(eyes[i].position, boxCastSize, 0f, direction);
                        RaycastHit2D boxCastBehindHit = Physics2D.BoxCast(raycastBehindStartPoint, boxCastSize, 0f, -direction);*/

            RaycastHit2D raycastFrontHit = Physics2D.Raycast(eyes[i].position, direction, viewRange, targetsLayer);
            RaycastHit2D raycastBehindHit = Physics2D.Raycast(raycastBehindStartPoint, -direction, viewRange, targetsLayer);

            Collider2D frontHitCollider = raycastFrontHit.collider;
            Collider2D behindHitCollider = raycastBehindHit.collider;

            if (frontHitCollider != null && frontHitCollider.tag == Tags.PLAYER)
            {
                currentTargetDirection = direction;
                StartCoroutine(TargetSpottedBehaviour(frontHitCollider.transform, eyes[i], direction));
                return;
            }
            else if (behindHitCollider != null && behindHitCollider.tag == Tags.PLAYER)
            {
                direction *= -1f;
                currentTargetDirection = direction;
                StartCoroutine(TargetSpottedBehaviour(behindHitCollider.transform, eyes[i], direction));
                return;
            }
        }

        if (targetSpotted)
        {
            targetSpotted = false;
            Invoke(nameof(ConfirmTargetLost), 1f);
        }
    }

    IEnumerator TargetSpottedBehaviour(Transform target, Transform eye, Vector2 direction)
    {
        targetSpotted = true;

        if (!canAttack || status.IsAttacking) yield break;

        status.IsAttacking = true;
        status.CanMove = false;

        enemyRb.velocity = new Vector2(0f, enemyRb.velocity.y / 2f);

        FlipSprite(direction.x);
        yield return new WaitForSeconds(beforeAttackDelay);
        FlipSprite(currentTargetDirection.x);

        ShootProjectile(eye, target);

        status.IsAttacking = false;
        StartCoroutine(AfterAttackCooldown());
        if (animator != null) animator.SetTrigger("attackStart");
    }

    void ConfirmTargetLost()
    {
        if (targetSpotted) return;

        status.CanMove = true;
    }

    IEnumerator AfterAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(afterAttackCooldown);
        canAttack = true;
    }

    void InterruptAttacking() //Called via event when enemy gets hit
    {
        CancelInvoke(nameof(SearchForTarget));
        InvokeRepeating(nameof(SearchForTarget), 0.5f, 0.1f);
    }

    void ShootProjectile(Transform fromWhere, Transform target)
    {
        GameObject projectile = ObjectPool.Instance.GetPooledObject(enemyProjectile.tag);
        if (projectile == null) return;

        projectile.transform.position = fromWhere.position;

        StartCoroutine(OnAttackEffect());
        AfterAttackRecoil();

        projectile.SetActive(true);
        projectile.GetComponent<Projectile>().StartMoving(target);
    }

    IEnumerator OnAttackEffect()
    {
        if (onAttackParticle != null) onAttackParticle.Play();

        float glowIntensinity = onAttackGlow.GlowIntensinity;
        Color glowColor = onAttackGlow.GlowColor * glowIntensinity;

        enemyMaterial.color = glowColor;

        while (glowIntensinity > idleGlow.GlowIntensinity)
        {
            glowIntensinity *= 0.5f;
            glowColor = onAttackGlow.GlowColor * glowIntensinity;
            enemyMaterial.color = glowColor;
            yield return new WaitForSeconds(0.05f);
        }
        enemyMaterial.color = idleGlow.Effect;
    }

    void AfterAttackRecoil()
    {
        float directionX;
        if (status.IsFacingLeft)
        {
            directionX = -1f;
        }
        else
        {
            directionX = 1f;
        }

        Vector2 recoil = new Vector2(attackRecoil.x * -1f * directionX, attackRecoil.y);
        enemyRb.AddForce(recoil, ForceMode2D.Impulse);
    }

    void StopSearchingForTarget()
    {
        CancelInvoke(nameof(SearchForTarget));
    }

    void ContinueSearchingForTarget()
    {
        InvokeRepeating(nameof(SearchForTarget), 0f, 0.1f);
    }

    void FixedUpdate()
    {
        if (!enemySprite.isVisible && inCameraViewRange)
        {
            inCameraViewRange = false;
            StopSearchingForTarget();
            return;
        }

        if (enemySprite.isVisible && !inCameraViewRange)
        {
            inCameraViewRange = true;
            ContinueSearchingForTarget();
            return;
        }
    }

    void OnDestroy()
    {
        health.GotHit -= InterruptAttacking;
    }

    void OnDrawGizmos()
    {
        foreach (Transform eye in eyes)
        {
            Vector2 endPoint;
            Vector2 behindEndPoint;
            Vector2 raycastBehindStartPoint;
            if (status.IsFacingLeft)
            {
                endPoint = new Vector2((eye.position.x + viewRange * -1f), eye.position.y);
                raycastBehindStartPoint = new Vector2((eye.position.x - (enemyCollider.bounds.extents.x * 2) * -1f), eye.position.y);

                behindEndPoint = new Vector2((raycastBehindStartPoint.x + viewRange), eye.position.y);
            }
            else
            {
                endPoint = new Vector2(eye.position.x + viewRange, eye.position.y);
                raycastBehindStartPoint = new Vector2((eye.position.x - (enemyCollider.bounds.extents.x * 2)), eye.position.y);

                behindEndPoint = new Vector2((raycastBehindStartPoint.x + viewRange * -1f), eye.position.y);
            }

            Gizmos.DrawLine(eye.transform.position, endPoint);
            Gizmos.DrawLine(raycastBehindStartPoint, behindEndPoint);
        }
    }
}