using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class managing single inventory slot in the inventory window.
/// </summary>
public class SingleInventorySlot : MonoBehaviour
{
    InventorySlotsDisplay inventorySlotsDisplay; //These references will be set up by slots display script
    InventoryObject displayedInventory;

    [Header("References")]
    [SerializeField] TextMeshProUGUI itemsAmount;
    [SerializeField] Image itemIcon;

    int slotId; //id refers to array index in inventory list of slots
    bool isEmpty;

    #region Getters

    public TextMeshProUGUI ItemsAmount { get => itemsAmount; }
    public Image ItemIcon { get => itemIcon; }
    public int SlotId { get => slotId; }
    public bool IsEmpty { get => isEmpty; }

    #endregion Getters

    public void UpdateSlotDisplay()
    {
        InventoryObject.InventorySlot slot = displayedInventory.InventorySlotsList[slotId];

        if (slot != null)
        {
            if (slot.Item != null)
            {
                isEmpty = false;
                ItemsAmount.text = slot.ItemsAmount.ToString();
                ItemIcon.enabled = true;
                ItemIcon.sprite = slot.Item.Icon;
            }
            else if (slot.Item == null || slot.ItemsAmount <= 0)
            {
                isEmpty = true;
                ItemsAmount.text = null;
                ItemIcon.sprite = null;
                itemIcon.enabled = false;
            }
        }
    }

    public void PrepareSlot(int id, InventorySlotsDisplay slotsMaster, InventoryObject displayedInventoryArg)
    {
        isEmpty = true;
        slotId = id;

        itemsAmount.text = null;
        itemIcon.sprite = null;

        inventorySlotsDisplay = slotsMaster;
        displayedInventory = displayedInventoryArg;
    }

    #region Mouse events

    public void PointerEnter() => inventorySlotsDisplay.OnEnter(this);

    public void PointerExit() => inventorySlotsDisplay.OnExit(this);

    public void BeginDrag() => inventorySlotsDisplay.OnDragStart(this);

    public void EndDrag() => inventorySlotsDisplay.OnDragStop(this);

    public void Drag() => inventorySlotsDisplay.OnDrag(this);

    public void PointerClick() => inventorySlotsDisplay.OnClick(this);

    #endregion Mouse events
}