using UnityEngine;

public class DestructibleContainer : MonoBehaviour, IDamageable
{
    [SerializeField] Transform ItemSpawnPoint;
    [SerializeField] Collider2D containerCollider;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ParticleSystem destroyParticle;
    [SerializeField] LootDropper lootDropper;

    public void TakeDamage(int damage) => Destroy();

    void Destroy()
    {
        destroyParticle.transform.parent = null;
        destroyParticle.Play();
        lootDropper.DropLoot();
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, Vector2 knockbackForce, Vector3 damagingObjectPosition)
    {
        Destroy();
    }

    public float GetFacingDirection()
    {
        throw new System.NotImplementedException();
    }
}