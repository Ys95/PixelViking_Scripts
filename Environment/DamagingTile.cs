using UnityEngine;

public class DamagingTile : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] Vector2 knockbackStrength;
    [SerializeField] bool affectsEnemies;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!affectsEnemies && collider.tag == Tags.ENEMY) return;
        if (collider.tag != Tags.PLAYER && collider.tag != Tags.ENEMY) return;

        IDamageable health = collider.GetComponent<IDamageable>();
        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

        if (health == null || rb == null) return;

        float directionX = health.GetFacingDirection();
        float directionY;

        if (rb.velocity.y==0f)
        {
            directionY = 1f;
        }
        else
        {
            directionY = -1f * (rb.velocity.normalized).y;
        }

        Vector2 tilePos = new Vector2(collider.transform.position.x + directionX, collider.transform.position.y);
        Vector2 knockbackForce = new Vector2(knockbackStrength.x, (knockbackStrength.y+(rb.velocity.y*-1f)) * directionY);

        health.TakeDamage(damageAmount, knockbackForce , tilePos);
    }
}