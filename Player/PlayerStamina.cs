using System.Collections;
using UnityEngine;

/// <summary>
/// Handles regeneration and usage of stamina
/// </summary>
public class PlayerStamina : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int maxStamina;
    [SerializeField] int regeneratedInTick;
    [SerializeField] float timeBetweenTicks;
    [SerializeField] float cooldownAfterStaminUse;
    [SerializeField] int stamina;

    bool pauseRegeneration;
    bool isRegeneratingStamina;

    [SerializeField] float cooldownTimer;

    IEnumerator pauseRegenCoroutine;

    public int Stamina { get => stamina; }
    public int MaxStamina { get => maxStamina; }

    void Awake()
    {
        stamina = maxStamina;
        pauseRegenCoroutine = PauseRegeneration();
    }

    public bool HasEnoughStamina(int neededAmount)
    {
        if (stamina >= neededAmount) return true;

        return false;
    }

    public bool IsStaminaFull()
    {
        if (stamina == maxStamina) return true;
        return false;
    }

    public void UseStamina(int amount)
    {
        stamina -= amount;
        if (stamina < 0) stamina = 0;

        StopCoroutine(pauseRegenCoroutine);
        pauseRegenCoroutine = PauseRegeneration();
        StartCoroutine(pauseRegenCoroutine);
    }

    public void RestoreStamina(int amount)
    {
        stamina += amount;

        if (stamina + amount > maxStamina)
        {
            stamina = maxStamina;
        }
    }

    public IEnumerator PauseRegeneration()
    {
        cooldownTimer = cooldownAfterStaminUse;
        pauseRegeneration = true;
        while (cooldownTimer > 0f)
        {
            yield return new WaitForSeconds(1f);
            cooldownTimer -= 1f;
        }
        pauseRegeneration = false;
    }

    IEnumerator RegenerateStamina()
    {
        isRegeneratingStamina = true;
        while (stamina < maxStamina)
        {
            if (!pauseRegeneration)
            {
                stamina += regeneratedInTick;
                yield return new WaitForSeconds(timeBetweenTicks);
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
        isRegeneratingStamina = false;
    }

    void FixedUpdate()
    {
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
        if (isRegeneratingStamina) return;
        if (stamina == maxStamina) return;

        StartCoroutine(RegenerateStamina());
    }
}