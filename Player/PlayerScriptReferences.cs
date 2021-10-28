using UnityEngine;

/// <summary>
/// Provides references to scripts.
/// </summary>
public class PlayerScriptReferences : MonoBehaviour
{
    [SerializeField] PlayerInputManager input;

    [Space]
    [Header("Movement")]
    [SerializeField] CharacterMovement movement;
    [SerializeField] PlayerGrab grab;
    [SerializeField] CharacterStatus status;

    [Header("Combat")]
    [SerializeField] CharacterStats stats;
    [SerializeField] PlayerAttack attack;
    [SerializeField] PlayerCombo combo;
    [SerializeField] PlayerStamina stamina;
    [SerializeField] PlayerHealth health;

    [Header("Inventory system")]
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] ConsumableItemsBuffs itemBuffs;

    [Header("Misc")]
    [SerializeField] PlayerInteract interact;
    [SerializeField] PlayerAnimations animations;
    [SerializeField] PlayerSounds sounds;

    public PlayerInputManager Input { get => input; }
    public CharacterMovement Movement { get => movement; }
    public PlayerGrab Grab { get => grab; }
    public CharacterStatus Status { get => status; }
    public CharacterStats Stats { get => stats; }
    public PlayerAttack Attack { get => attack; }
    public PlayerCombo Combo { get => combo; }
    public PlayerStamina Stamina { get => stamina; }
    public PlayerHealth Health { get => health; }
    public PlayerInventory PlayerInventory { get => playerInventory; }
    public ConsumableItemsBuffs ItemBuffs { get => itemBuffs; }
    public PlayerInteract Interact { get => interact; }
    public PlayerAnimations Animations { get => animations; }
    public PlayerSounds Sounds { get => sounds; }
}