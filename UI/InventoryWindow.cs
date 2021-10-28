using UnityEngine;

public class InventoryWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerInventory displayedInventory;
    [SerializeField] UIManager uiManager;
    [SerializeField] ItemDetailsWindow itemDetailsWindow;
    [SerializeField] CanvasGroup canvasGroup;

    bool mouseHoveringOverInventoryWindow;

    public bool MouseHoveringOverInventoryWindow { get => mouseHoveringOverInventoryWindow; }
    public ItemDetailsWindow ItemDetailsWindow { get => itemDetailsWindow; }
    public PlayerInventory DisplayedInventory { get => displayedInventory; }

    void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
    }

    public void OpenInventory(bool open)
    {
        if (open)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            return;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        itemDetailsWindow.HideWindow();
    }

    #region Mouse events

    public void OnEnter()
    {
        mouseHoveringOverInventoryWindow = true;
    }

    public void OnExit()
    {
        mouseHoveringOverInventoryWindow = false;
    }

    #endregion Mouse events
}