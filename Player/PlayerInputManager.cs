using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class handling inputs. Responsible for switching action maps, calling methods, setting corrent Y and X input values.
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputActionAsset playerInputAsset;
    [SerializeField] private CharacterMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerGrab playerGrab;
    [SerializeField] private PlayerInventory playerInventoryManager;

    [Space]
    [SerializeField] private InputActionMap ground;
    [SerializeField] private InputActionMap grab;

    static readonly string LAND_ACTION_MAP = "Land";
    static readonly string GRAB_ACTION_MAP = "Grab";

    void Awake()
    {
        ground = playerInputAsset.FindActionMap(LAND_ACTION_MAP);
        grab = playerInputAsset.FindActionMap(GRAB_ACTION_MAP);

        GameManager.GamePaused += DisableInput;
    }

    public void ActivateGrabActionMap()
    {
        playerInput.SwitchCurrentActionMap(GRAB_ACTION_MAP);
        Debug.Log("Grab action map activated");
    }

    public void ActivateGroundActionMap()
    {
        playerInput.SwitchCurrentActionMap(LAND_ACTION_MAP);
        Debug.Log("Ground action map activated");
    }

    void DisableInput(bool disable) => EnableInput(!disable);

    public void EnableInput(bool enable)
    {
        if (enable)
        {
            playerInput.ActivateInput();
            return;
        }
        playerInput.DeactivateInput();
    }

    void SpamInLog(string spam)
    {
        Debug.Log(spam);
    }

    #region GroundActionMap

    void OnMovement(InputValue movementValue)
    {
        float inputX = movementValue.Get<float>();
        playerMovement.InputAxisX = inputX;
    }

    void OnJump()
    {
        Debug.Log("jump");
        if (playerGrab.LookForGrabable() == false)
        {
            playerMovement.Jump();
        }
    }

    void OnCancelJump()
    {
        Debug.Log("Cancel");
        playerMovement.CancelJump();
    }

    void OnAttack(InputValue attack) => playerAttack.PrepareAttack();

    void OnInteract(InputValue interact) => playerInteract.InteractWithObject();

    void OnQuickSlot1() => playerInventoryManager.UseItem(0);

    void OnQuickSlot2() => playerInventoryManager.UseItem(1);

    void OnQuickSlot3() => playerInventoryManager.UseItem(2);

    void OnQuickSlot4() => playerInventoryManager.UseItem(3);

    #endregion GroundActionMap

    #region GrabActionMap

    void OnWiggling(InputValue wiggleValue)
    {
        playerGrab.InputAxisX = wiggleValue.Get<float>();
    }

    void OnJumpOff(InputValue jumpOff) => playerGrab.StopGrabing();

    #endregion GrabActionMap

    void OnDestroy()
    {
        GameManager.GamePaused -= DisableInput;
    }
}