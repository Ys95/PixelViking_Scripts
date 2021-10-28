using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Light2D[] lights2d;
    [SerializeField] SpriteRenderer lightSourceSprite;

    [Header("Parameters")]
    [SerializeField] bool intensinityFlicker;
    [SerializeField] bool radiusFlicker;

    [Space]
    [SerializeField] float LightRadiusMin;
    [SerializeField] float LightRadiusMax;

    [Space]
    [SerializeField] float intensinityMin;
    [SerializeField] float intensinityMax;

    [Space]
    [SerializeField] float fallOffStrengthMin;
    [SerializeField] float fallOffStrengthMax;

    [Space]
    [SerializeField] float TimeBetweenFlickersMin;
    [SerializeField] float TimeBetweenFlickersMax;

    IEnumerator flickerCoroutine;
    bool isVisible;

    void Awake()
    {
        AssignCoroutine();
    }

    void AssignCoroutine()
    {
        if (intensinityFlicker) flickerCoroutine = FlickeringEffectIntensinity();
        if (radiusFlicker) flickerCoroutine = FlickeringEffectRadius();

        StartCoroutine(flickerCoroutine);
    }

    IEnumerator FlickeringEffectRadius()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(TimeBetweenFlickersMin, TimeBetweenFlickersMax));

            for (int i = 0; i < lights2d.Length; i++)
            {
                lights2d[i].pointLightOuterRadius = Random.Range(LightRadiusMin, LightRadiusMax);
            }
        }
    }

    IEnumerator FlickeringEffectIntensinity()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(TimeBetweenFlickersMin, TimeBetweenFlickersMax));

            for (int i = 0; i < lights2d.Length; i++)
            {
                lights2d[i].intensity = Random.Range(intensinityMin, intensinityMax);
            }
        }
    }

    void Update()
    {
        if (lightSourceSprite == null) return;

        if (!lightSourceSprite.isVisible && isVisible)
        {
            isVisible = false;
            StopCoroutine(flickerCoroutine);
        }

        if (lightSourceSprite.isVisible && !isVisible)
        {
            isVisible = true;
            AssignCoroutine();
        }
    }
}