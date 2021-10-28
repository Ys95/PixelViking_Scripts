using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "item_spell_itemname", menuName = "Inventory System/Items/Spell")]
public class SpellItem : ItemObject
{
    [SerializeField] GameObject spellProjectile;
    [SerializeField] GameObject onCastParticles;

    [Space]
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileLifetime;
    [SerializeField] int projectileDamage;

    public void CastSpell(Transform location, float direction)
    {
        GameObject prefab = ObjectPool.Instance.GetPooledObject(spellProjectile.tag);

        if (prefab == null)
        {
            Debug.LogWarning("Spell not in object pool");
            return;
        }

        prefab.transform.position = location.position;
        Projectile projectile = prefab.GetComponent<Projectile>();

        projectile.IsPlayerProjectile(true);

        projectile.Damage = projectileDamage;
        projectile.LifeTime = projectileLifetime;
        projectile.Speed = projectileSpeed;

        prefab.SetActive(true);

        projectile.StartMoving(direction);
    }

    public IEnumerator SpawnParticles(Transform location)
    {
        GameObject prefab = Instantiate(onCastParticles, location);
        ParticleSystem particles = prefab.GetComponent<ParticleSystem>();
        particles.Play();
        yield return new WaitForSeconds(particles.main.duration + 0.5f);
        Destroy(prefab);
    }

    public override void CreateAutoDescription()
    {
        autoDescription = string.Concat("Shots out projectile that deals " + projectileDamage + " damage.");
    }

    public override void SetProperties()
    {
        //
    }

    public override void SetType()
    {
        type = ItemType.Spell;
    }
}