using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] PlayerInput uiInput;

    [Header("Hud")]
    [SerializeField] Canvas hudCanvas;

    [Header("Inventory")]
    [SerializeField] InventoryWindow inventoryWindow;

    [Header("Menus")]
    [SerializeField] UiMenu pauseMenu;
    [SerializeField] UiMenu saveMenu;
    [SerializeField] UiMenu loadMenu;
    [SerializeField] UiMenu optionsMenu;
    [SerializeField] UiMenu deathScreen;

    [Space]
    [SerializeField] bool inMainMenu;

    UiMenu currentlyOpen;

    bool isInventoryWindowOpen;

    #region Getters

    public UiMenu PauseMenu { get => pauseMenu; }
    public UiMenu SaveMenu { get => saveMenu; }
    public UiMenu LoadMenu { get => loadMenu; }
    public UiMenu OptionsMenu { get => optionsMenu; }
    public UiMenu DeathScreen { get => deathScreen; }

    #endregion Getters

    [System.Serializable]
    public class UiMenu
    {
        [SerializeField] GameObject menu;
        bool isOpen;

        public bool IsOpen { get => isOpen; }

        public void Open()
        {
            menu.SetActive(true);
            isOpen = true;
        }

        public void Close()
        {
            menu.SetActive(false);
            isOpen = false;
        }
    }

    void Awake()
    {
        LoadScaleSettings();

        PlayerHealth.PlayerIsDead += PlayerDied;
        LevelManager.LoadingStarted += DisableInput;
        LevelManager.LoadingFinished += ActivateInput;

        if(inMainMenu)
        {
            Cursor.visible = true;
        }
    }

    void PlayerDied() => Invoke("ShowDeathScreen", 1f);

    void ShowDeathScreen() => deathScreen.Open();

    public void CloseCurrentlyOpen() => CloseMenu(currentlyOpen);

    public void LoadScaleSettings()
    {
        if (!inMainMenu)
        {
            float scaleFactor = 1f + (1f - PlayerSettings.UiScale.Load());
            Vector2 defaultScale = new Vector2(1920f, 1080f);

            Vector2 newScale = new Vector2(1920f * scaleFactor, 1080f * scaleFactor);

            hudCanvas.GetComponent<CanvasScaler>().referenceResolution = newScale;
        }
    }

    public void OpenMenu(UiMenu menu)
    {
        if (menu == null) return;

        menu.Open();
        currentlyOpen = menu;
    }

    public void SavePointMenu()
    {
        void OnSaveMenuClosed()
        {
            GameManager.Pause(false);
            SaveAndLoadMenu.MenuClosed -= OnSaveMenuClosed;
        }

        OpenMenu(saveMenu);
        GameManager.Pause(true);
        SaveAndLoadMenu.MenuClosed += OnSaveMenuClosed;
    }

    public void CloseMenu(UiMenu menu)
    {
        menu.Close();
        currentlyOpen = null;
    }

    public void OpenInventory(bool open)
    {
        if (open)
        {
            Cursor.visible = true;
            inventoryWindow.OpenInventory(true);
            isInventoryWindowOpen = true;
            return;
        }
        Cursor.visible = false;
        isInventoryWindowOpen = false;
        inventoryWindow.OpenInventory(false);
    }

    public void GoToMainMenu()
    {
        LevelManager.LoadLevel(LevelManager.MAIN_MENU);
    }

    void OnDisable()
    {
        PlayerHealth.PlayerIsDead -= PlayerDied;
        LevelManager.LoadingStarted -= DisableInput;
        LevelManager.LoadingFinished -= ActivateInput;
    }

    #region Input

    void ActivateInput() => uiInput.ActivateInput();

    void DisableInput() => uiInput.DeactivateInput();

    public void OnOpenInventory()
    {
        if (inMainMenu) return;
        if (deathScreen.IsOpen) return;

        if (!isInventoryWindowOpen)
        {
            OpenInventory(true);
            return;
        }
        OpenInventory(false);
    }

    public void OnRespawn()
    {
        if (deathScreen.IsOpen)
        {
            SaveSystem.Instance.LoadTempSave();
            return;
        }
    }

    public void OnESC()
    {
        if (deathScreen.IsOpen) return;

        if (isInventoryWindowOpen)
        {
            OpenInventory(false);
        }

        if (currentlyOpen != null && !currentlyOpen.Equals(null)) //one nullcheck not enough
        {
            CloseMenu(currentlyOpen);
            return;
        }

        if (!pauseMenu.IsOpen && !inMainMenu)
        {
            pauseMenu.Open();
            return;
        }

        if (pauseMenu.IsOpen)
        {
            pauseMenu.Close();
            return;
        }
    }

    #endregion Input
}