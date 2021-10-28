using UnityEngine;

/// <summary>
/// Class managing characters stats
/// </summary>
public class CharacterStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Stats values;

    [Header("Current stats")]
    [SerializeField] int health;

    [SerializeField] int damageReduction;
    [SerializeField] int attackPower;
    [SerializeField] Vector2 knockbackStrength;

    #region Getters

    public int Health { get => health; }
    public int AttackPower { get => attackPower; }
    public int DamageReduction { get => damageReduction; }
    public Vector2 KnockbackStrength { get => knockbackStrength; }
    public Stats Values { get => values; }

    #endregion Getters

    void Awake()
    {
        LoadDefaultStats();
    }

    void LoadDefaultStats()
    {
        health = values.MaxHealth;
        attackPower = values.AttackPower;
        damageReduction = values.DamageReduction;
        knockbackStrength = values.KnockbackStrength;
    }

    public void AddHealth(int value)
    {
        health += value;
        if (health > values.MaxHealth)
        {
            health = values.MaxHealth;
        }
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    public void AddAttackPower(int value)
    {
        attackPower += value;
    }

    public void AddDamageReduction(int value)
    {
        damageReduction += value;
    }
}