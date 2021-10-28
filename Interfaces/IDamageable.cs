using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage, Vector2 knockbackForce, Vector3 damagingObjectPosition);

    public float GetFacingDirection();
}