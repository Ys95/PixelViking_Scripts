using UnityEngine;

[CreateAssetMenu(fileName = "stats_name", menuName = "Character/Stats")]
public class Stats : ScriptableObject
{
    [SerializeField] int maxHealth;
    [SerializeField] int damageReduction;
    [SerializeField] int attackPower;
    [SerializeField] Vector2 knockbackStrength;

    public int MaxHealth { get => maxHealth; }
    public int AttackPower { get => attackPower; }
    public int DamageReduction { get => damageReduction; }
    public Vector2 KnockbackStrength { get => knockbackStrength; }
}