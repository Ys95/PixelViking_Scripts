using System.Collections;
using UnityEngine;

public class BuffVisualEffects : MonoBehaviour
{
    [SerializeField] Transform buffEffectSpawn;

    [Space]
    [SerializeField] Sprite attackBuffIcon;
    [SerializeField] Sprite defenseBuffIcon;
    [SerializeField] Sprite regenerationBuffIcon;
    [SerializeField] ParticleSystem buffParticle;
    [SerializeField] ParticleSystem healingParticle;

    [Space]
    [SerializeField] float effectLifetime;
    [SerializeField] float floatSpeed;

    public void StartEffect(ConsumableEffect effect)
    {
        switch (effect)
        {
            case ConsumableEffect.Damage:
                {
                    StartCoroutine(PlayBuffEffect(attackBuffIcon, buffEffectSpawn.position));
                    return;
                }
            case ConsumableEffect.Armor:
                {
                    StartCoroutine(PlayBuffEffect(defenseBuffIcon, buffEffectSpawn.position));
                    return;
                }
            case ConsumableEffect.Regeneration:
                {
                    StartCoroutine(PlayBuffEffect(regenerationBuffIcon, buffEffectSpawn.position));
                    return;
                }
            case ConsumableEffect.Healing:
                {
                    PlayHealingParticles();
                    return;
                }
            default:
                {
                    return;
                }
        }
    }

    void PlayHealingParticles() => healingParticle.Play();

    IEnumerator PlayBuffEffect(Sprite buffIcon, Vector3 position)
    {
        if (buffParticle != null)
        {
            buffParticle.Play();
        }
        GameObject floatingIconObject = ObjectPool.Instance.GetPooledObject(Tags.FLOATING_ICON);
        floatingIconObject.transform.position = position;

        SpriteRenderer floatingIconSprite = floatingIconObject.GetComponent<SpriteRenderer>();
        floatingIconSprite.sprite = buffIcon;

        floatingIconObject.SetActive(true);

        float timer = effectLifetime;
        while (timer > 0f)
        {
            floatingIconObject.transform.position = floatingIconObject.transform.position + new Vector3(0f, floatSpeed * Time.fixedDeltaTime, 0f);
            yield return new WaitForFixedUpdate();
            timer -= Time.fixedDeltaTime;
        }
        floatingIconObject.SetActive(false);
    }
}