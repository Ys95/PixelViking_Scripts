using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Slider staminaBar;
    [SerializeField] CanvasGroup staminaBarCanvasGroup;
    [SerializeField] PlayerStamina playerStamina;

    bool hideBarEffectRunning;
    Transform originalTransform; //to ignore player transform.

    private void Awake()
    {
        HideStaminaBar(true);
        staminaBar.maxValue = playerStamina.MaxStamina;
    }

    void HideStaminaBar(bool hide)
    {
        if (hide)
        {
            staminaBarCanvasGroup.alpha = 0f;
            return;
        }
        staminaBarCanvasGroup.alpha = 1f;
    }

    IEnumerator HideStaminaBarEffect()
    {
        hideBarEffectRunning = true;
        yield return new WaitForSeconds(1f);

        if (!playerStamina.IsStaminaFull()) yield return 0;

        HideStaminaBar(true);
        hideBarEffectRunning = false;
        yield return 0;
    }

    void Update()
    {
        transform.localScale = transform.parent.localScale;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.parent.rotation.z * -1.0f);

        staminaBar.value = playerStamina.Stamina;
        if (playerStamina.IsStaminaFull())
        {
            if (!hideBarEffectRunning) StartCoroutine(HideStaminaBarEffect());
            return;
        }
        HideStaminaBar(false);
    }
}