using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    bool canMove;
    bool isGrounded;
    bool isOnSlope;
    bool isJumping;
    bool isFacingLeft;
    bool isGrabing;

    bool canBeDamaged;
    bool canAttack;
    bool isAttacking;

    bool isDead;

    public bool CanMove { get => canMove; set => canMove = value; }
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public bool IsOnSlope { get => isOnSlope; set => isOnSlope = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool CanBeDamaged { get => canBeDamaged; set => canBeDamaged = value; }
    public bool IsFacingLeft { get => isFacingLeft; set => isFacingLeft = value; }
    public bool IsGrabing { get => isGrabing; set => isGrabing = value; }

    void Awake()
    {
        canMove = true;
        canBeDamaged = true;
        canAttack = true;
    }
}