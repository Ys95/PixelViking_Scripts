using TMPro;
using UnityEngine;

public class SaveAndLoadMenu : MonoBehaviour
{
    [Space]
    [SerializeField] bool isLoadMenu;

    [Header("Componenets")]
    [SerializeField] SaveSlotUI[] saveSlots;
    [SerializeField] GameObject savePopup;
    [SerializeField] TMP_InputField saveFileNameInput;

    [Space]
    [SerializeField] GameObject saveBtn;
    [SerializeField] GameObject loadBtn;

    public delegate void OnMenuClosed();
    public static event OnMenuClosed MenuClosed;

    int selectedSlot = -1;

    void OnEnable()
    {
        if (isLoadMenu)
        {
            loadBtn.SetActive(true);
            saveBtn.SetActive(false);
        }
        else
        {
            loadBtn.SetActive(false);
            saveBtn.SetActive(true);
        }
        UpdateSlots();
    }

    void OnDisable()
    {
        selectedSlot = -1;
        DisableSelectionOutline();
        savePopup.SetActive(false);

        loadBtn.SetActive(false);
        saveBtn.SetActive(false);

        MenuClosed?.Invoke();
    }

    void DisableSelectionOutline()
    {
        foreach (SaveSlotUI slot in saveSlots)
        {
            slot.SelectionOutline.enabled = false;
        }
    }

    void UpdateSlots()
    {
        foreach (SaveSlotUI slot in saveSlots)
        {
            slot.UpdateSlot();
        }
    }

    public void SaveSlotClick(int slot)
    {
        DisableSelectionOutline();
        selectedSlot = slot;

        saveSlots[slot].SelectionOutline.enabled = true;

        if (isLoadMenu && !loadBtn.activeInHierarchy)
        {
            loadBtn.SetActive(true);
            return;
        }
        else if (!isLoadMenu && !saveBtn.activeInHierarchy)
        {
            saveBtn.SetActive(true);
        }
    }

    public void SaveBtnClick()
    {
        savePopup.SetActive(true);
    }

    public void LoadBtnClick()
    {
        if (selectedSlot == -1) return;

        SaveSystem.Instance.LoadGame(selectedSlot);
    }

    public void PopupWindowOkBtnClick()
    {
        string saveName = saveFileNameInput.text;

        if (string.IsNullOrWhiteSpace(saveName))
        {
            saveName = "Unnamed";
        }

        SaveSystem.Instance.CreateSaveFile(selectedSlot, saveName);

        saveFileNameInput.text = default;
        savePopup.SetActive(false);
        UpdateSlots();
    }
}