using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for displaying player owned items in the inventory UI.
/// </summary>
public class InventorySlotsDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] PlayerInventory playerInventoryManager;
    [SerializeField] SingleQuickSlot[] quickSlotsUI;
    [SerializeField] SingleInventorySlot[] inventorySlotsUI;
    [SerializeField] InventoryWindow inventoryWindow;
    [SerializeField] TextMeshProUGUI goldAmountDisplay;

    MouseItem mouseItem = new MouseItem();

    /// <summary>
    /// MouseItem contains information about an object created when player drags inventory slot.
    /// It represents item that is dragged from the slot.
    /// </summary>
    class MouseItem
    {
        public GameObject Object;
        public RectTransform RectTransform;
        public Image Image; //icon of the dragged item
        public SingleInventorySlot HoveredOverSlot; //inventory slot

        public void Create(Transform transform)
        {
            Object = new GameObject();
            RectTransform = Object.AddComponent<RectTransform>();
            RectTransform.sizeDelta = new Vector2(50, 50);
            Image = Object.AddComponent<Image>();
            Object.transform.SetParent(transform.parent);
        }

        public void Clear()
        {
            Destroy(Object);
            HoveredOverSlot = null;
        }
    }

    void Start()
    {
        CreateInventoryDisplay();
    }

    void CreateInventoryDisplay()
    {
        for (int i = 0; i < inventorySlotsUI.Length; i++)
        {
            inventorySlotsUI[i].PrepareSlot(i, this, inventoryWindow.DisplayedInventory.Inventory);
        }
    }

    void UpdateInventoryDisplay()
    {
        foreach (SingleInventorySlot slot in inventorySlotsUI)
        {
            slot.UpdateSlotDisplay();
        }
        goldAmountDisplay.text = inventoryWindow.DisplayedInventory.Inventory.Gold.ToString(); //Update coins amount
    }

    void Update()
    {
        UpdateInventoryDisplay();
    }

    #region Mouse events

    public void OnClick(SingleInventorySlot uiSlot) //When inventory slot is clicked
    {
        if (!uiSlot.IsEmpty)
        {
            var slotId = uiSlot.SlotId;
            inventoryWindow.ItemDetailsWindow.DisplayItemDetails(inventoryWindow.DisplayedInventory.Inventory.InventorySlotsList[slotId].Item); //display item details in window
        }
    }

    public void OnEnter(SingleInventorySlot uiSlot) //When mouse pointer enters inventory slot
    {
        mouseItem.HoveredOverSlot = uiSlot; //Remember slot that mouse hovered over
    }

    public void OnExit(SingleInventorySlot uiSlot) //When mouse pointer exits inventory slot
    {
        mouseItem.HoveredOverSlot = null;
    }

    public void OnDragStart(SingleInventorySlot uiSlot) //When starting dragging inventory slot
    {
        if (!uiSlot.IsEmpty) //if there is item in slot, change mouseItem image to dragged item image
        {
            mouseItem.Create(transform);//Create gameobject representing dragged item
            mouseItem.Image.sprite = uiSlot.ItemIcon.sprite;
            mouseItem.Image.raycastTarget = false;
            mouseItem.HoveredOverSlot = uiSlot;
        }
    }

    public void OnDragStop(SingleInventorySlot uiSlot)  //When stopping draggin inventory slot
    {
        if (uiSlot.IsEmpty) return;

        if (mouseItem.HoveredOverSlot != null) //if mouse hovers over inventory slot - drag item there/swap items
        {
            Debug.Log("Original slot: " + uiSlot.SlotId);
            Debug.Log("Destination slot: " + mouseItem.HoveredOverSlot.SlotId);
            inventoryWindow.DisplayedInventory.Inventory.SwapSlots(uiSlot.SlotId, mouseItem.HoveredOverSlot.SlotId);
        }
        else if (!inventoryWindow.MouseHoveringOverInventoryWindow) //if mouse hovers outside inventory slot and outside inventory window - throw item away
        {
            var item = inventoryWindow.DisplayedInventory.Inventory.InventorySlotsList[uiSlot.SlotId].Item;
            var itemsAmount = inventoryWindow.DisplayedInventory.Inventory.InventorySlotsList[uiSlot.SlotId].ItemsAmount;

            playerInventoryManager.ThrowItem(item, itemsAmount);
            inventoryWindow.DisplayedInventory.Inventory.RemoveAllItems(item);
        }

        mouseItem.Clear();
    }

    public void OnDrag(SingleInventorySlot uiSlot) //While dragging inventory slot
    {
        if (mouseItem.Object != null) //Update mouseItem position
        {
            mouseItem.RectTransform.position = Input.mousePosition;
        }
    }

    #endregion Mouse events
}