using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleQuickSlot : MonoBehaviour
{
    [Header("Components references")]
    [SerializeField] InventoryObject inventory;
    [SerializeField] int linkedInvSlot;

    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemsAmountDisplay;
    [SerializeField] InventorySlotsDisplay inventorySlotsDisplay;

    public void UpdateQuickSlot()
    {
        if (inventory.InventorySlotsList[linkedInvSlot].Item == null)
        {
            WipeSlot();
            return;
        }
        itemIcon.enabled = true;
        itemIcon.sprite = inventory.InventorySlotsList[linkedInvSlot].Item.Icon;
        itemsAmountDisplay.text = inventory.InventorySlotsList[linkedInvSlot].ItemsAmount.ToString();
    }

    void WipeSlot()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        itemsAmountDisplay.text = 0.ToString();
    }

    void Update()
    {
        UpdateQuickSlot();
    }
}