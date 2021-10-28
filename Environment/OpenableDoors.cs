using UnityEngine;

public class OpenableDoors : MonoBehaviour, IInteractable
{
    [Header("Components")]
    [SerializeField] BoxCollider2D doorsCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite openedDoorsSprite;

    [Space]
    [SerializeField] ItemObject doorsKey;
    [SerializeField] bool nextStageDoors;

    bool isOpen;

    public bool WasInteractedWith { get => isOpen; }

    void ProceedToNextStage() => LevelManager.LoadNextLevel();

    public void Interact(GameObject whoInteracts)
    {
        PlayerInventory playerInventory = whoInteracts.GetComponent<PlayerInventory>();

        if (!playerInventory.Inventory.HasItem(doorsKey)) return;

        if (nextStageDoors)
        {
            ProceedToNextStage();
            return;
        }

        playerInventory.Inventory.RemoveItem(doorsKey, 1);
        OpenDoors();
    }

    void OpenDoors()
    {
        SoundManager.Instance.PlaySound(SoundCategory.Misc, SoundEffect.misc_Interaction, transform);
        doorsCollider.enabled = false;
        spriteRenderer.sprite = openedDoorsSprite;
        isOpen = true;
    }

    public void LoadState(bool open)
    {
        if (open)
        {
            OpenDoors();
        }
    }
}