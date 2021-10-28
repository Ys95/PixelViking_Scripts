using UnityEngine;

public class LockedChest : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] Animator animator;
    [SerializeField] Collider2D chestCollider;

    [Space]
    [SerializeField] ItemObject key;
    [SerializeField] LootDropper lootDropper;

    bool isOpen;

    public bool WasInteractedWith { get => isOpen; }

    public void Interact(GameObject whoInteracts)
    {
        if (whoInteracts.tag != Tags.PLAYER) return;

        PlayerInventory playerInventory = whoInteracts.GetComponent<PlayerInventory>();
        if (playerInventory == null) return;

        Debug.Log("Interacting with locked chest");

        bool playerHasKey = playerInventory.Inventory.HasItem(key);
        if (!isOpen && playerHasKey)
        {
            OpenChest();
            playerInventory.Inventory.RemoveItem(key, 1);

            lootDropper.DropLoot();
        }
    }

    public void LoadState(bool open)
    {
        if (open)
        {
            OpenChest();
        }
    }

    public void OpenChest()
    {
        SoundManager.Instance.PlaySound(SoundCategory.Misc, SoundEffect.misc_Interaction, transform);
        animator.SetBool("chestOpened", true);
        chestCollider.enabled = false;
        isOpen = true;
    }
}