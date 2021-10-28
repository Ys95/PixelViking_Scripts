using UnityEngine;

/// <summary>
/// Handles setting parameters in player animation controller.
/// </summary>
public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] PlayerScriptReferences player;

    [Header("Components")]
    [SerializeField] Animator animator;

    public void TriggerAttack()
    {
        animator.SetTrigger("attack");
    }

    public void TriggerComboAttack()
    {
        animator.SetTrigger("comboAttack");
    }

    public void TriggerGrab(bool isGrabbing)
    {
        if (isGrabbing)
        {
            animator.SetTrigger("startGrab");
            return;
        }
        animator.SetTrigger("stopGrab");
    }

    void FixedUpdate()
    {
        if (player.Movement.InputAxisX != 0f || player.Grab.InputAxisX != 0f)
        {
            animator.SetBool("inputX", true);
        }
        else
        {
            animator.SetBool("inputX", false);
        }
        animator.SetBool("isGrounded", player.Status.IsGrounded);
        animator.SetBool("isJumping", player.Status.IsJumping);
        animator.SetFloat("velocity", player.Movement.CharacterVelocity.x);
    }
}