using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailsWindow : MonoBehaviour
{
    [SerializeField] InventoryWindow inventoryWindow;

    [Space]
    [Header("Component references")]
    [SerializeField] Canvas canvas;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDescription;
    [SerializeField] Button useButton;
    [SerializeField] CanvasGroup canvasGroup;

    [Space]
    [SerializeField] ItemObject displayedItem;

    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        displayedItem = null;
    }

    void OnValidate()
    {
        if (displayedItem == null) return;
        DisplayItemDetails(displayedItem);
    }

    public void DisplayItemDetails(ItemObject item)
    {
        if (item != null)
        {
            displayedItem = item;

            if (item.Type == ItemType.Consumable)
            {
                useButton.interactable = true;
            }
            else
            {
                useButton.interactable = false;
            }

            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            itemIcon.sprite = item.Icon;
            itemName.text = item.DisplayName;
            itemDescription.text = item.GenerateDisplayedDescription();
        }
    }

    public void HideWindow()
    {
        displayedItem = null;
        useButton.interactable = false;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void UseButtonClick()
    {
        inventoryWindow.DisplayedInventory.UseItem(displayedItem);

        bool hasMoreItems = inventoryWindow.DisplayedInventory.Inventory.HasItem(displayedItem);
        if (!hasMoreItems)
        {
            HideWindow();
        }
    }
}