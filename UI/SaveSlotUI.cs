using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    [SerializeField] private SaveAndLoadMenu saveMenu;

    [Space]
    [SerializeField] private int slotId;

    [Space]
    [SerializeField] private Image selectionOutline;
    [SerializeField] private TextMeshProUGUI lvlNumber;
    [SerializeField] private TextMeshProUGUI saveFileName;
    [SerializeField] private TextMeshProUGUI saveFileCreationDate;

    #region Getters

    public Image SelectionOutline { get => selectionOutline; }
    public TextMeshProUGUI LvlNumber { get => lvlNumber; }
    public TextMeshProUGUI SaveFileName { get => saveFileName; }
    public TextMeshProUGUI SaveFileCreationDate { get => saveFileCreationDate; }

    #endregion Getters

    public void SlotClicked() => saveMenu.SaveSlotClick(slotId);

    public void UpdateSlot()
    {
        SaveData saveSlot = SaveSystem.Instance.SaveSlots[slotId];

        if (saveSlot == null || saveSlot.IsEmpty) return;

        lvlNumber.text = saveSlot.LevelId.ToString();
        saveFileName.text = saveSlot.FileName;
        saveFileCreationDate.text = saveSlot.FileCreationDate;
    }
}