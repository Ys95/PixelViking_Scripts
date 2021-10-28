using UnityEngine;

public class DamagingTile : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] Vector2 knockbackStrength;
    [SerializeField] bool affectsEnemies;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!affectsEnemies && collision.collider.tag == Tags.ENEMY) return;
        if (collision.collider.tag != Tags.PLAYER && collision.collider.tag != Tags.ENEMY) return;

        IDamageable health = collision.collider.GetComponent<IDamageable>();
        if (health == null) return;

        float directionY = 1f;
        float directionX = health.GetFacingDirection();

        if (collision.collider.transform.position.y < transform.position.y)
        {
            directionY = -1f;
        }

        Vector2 tilePos = new Vector2(collision.collider.transform.position.x + directionX, collision.collider.transform.position.y);
        Vector2 knockbackForce = new Vector2(knockbackStrength.x, knockbackStrength.y * directionY);

        health.TakeDamage(damageAmount, knockbackForce, tilePos);
    }
}