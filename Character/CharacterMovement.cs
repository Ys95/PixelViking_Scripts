using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] CharacterStatus status;

    [Header("Componenets")]
    [SerializeField] Rigidbody2D characterRigidbody;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D characterCollider;

    [Header("Ground movement")]
    [SerializeField] float movementSpeed;

    [SerializeField] Vector2 maxGroundVelocity;
    [SerializeField] Vector2 maxAirVelocity;
    [SerializeField] float floatingForce;
    [SerializeField] float jumpForce;
    [Range(0f, 100f)] [SerializeField] float onSlopeMovementForceMultiplier;

    [Space]
    [SerializeField] bool applyFallMultiplier;

    [SerializeField] float fallMultiplier;

    [Header("Ground logic")]
    [SerializeField] LayerMask groundLayer; //Target layer for groundcheck

    [Range(0f, 0.5f)] [SerializeField] float groundCheckRayExtraLength;
    [Range(0f, 100f)] [SerializeField] float rotationSensivity;
    [Range(0f, 20f)] [SerializeField] float rotationSmoothing;

    float inputAxisX;
    float inputAxisY;
    Vector2 characterVelocity;

    public delegate void CharacterLanding();

    public event CharacterLanding CharacterLanded;

    public delegate void CharacterJump();

    public event CharacterLanding CharacterJumped;

    float groundNormal;

    public float InputAxisX { get => inputAxisX; set => inputAxisX = value; } //value will be set by input script
    public float InputAxisY { get => inputAxisY; set => inputAxisY = value; } //value will be set by input script

    #region Getters

    public Vector2 MaxGroundVelocity { get => maxGroundVelocity; }
    public Vector2 MaxAirVelocity { get => maxAirVelocity; }
    public Vector2 CharacterVelocity { get => characterVelocity; }

    #endregion Getters

    void Awake()
    {
        CharacterLanded += ReduceLandingVelocity;
    }

    void Move()
    {
        float inAirPenalty = 1f;
        if (!status.IsGrounded && !status.IsGrabing)
        {
            inAirPenalty = 0.5f;
        }
        Vector2 movementForce = new Vector2(InputAxisX * movementSpeed * inAirPenalty, 0f);
        Vector2 flyingForce = new Vector2(0f, InputAxisY * floatingForce);

        float slopeMovement = Mathf.Abs(groundNormal);
        if (inputAxisX != 0f)
        {
            slopeMovement *= onSlopeMovementForceMultiplier;
            movementForce.x += slopeMovement;
        }

        characterRigidbody.AddForce(movementForce);
        characterRigidbody.AddForce(flyingForce);

        if (status.IsGrounded && !status.IsGrabing) LimitGroundVelocity();
        if (!status.IsGrounded && !status.IsGrabing) LimitInAirVelocity();
    }

    void ReduceLandingVelocity()
    {
        Vector2 newVelocity = new Vector2(characterRigidbody.velocity.x, characterRigidbody.velocity.y/20f);
        characterRigidbody.velocity = newVelocity;
    }

    void LimitGroundVelocity()
    {
        float velocityX = Mathf.Clamp(characterRigidbody.velocity.x, -maxGroundVelocity.x, maxGroundVelocity.x);

        characterRigidbody.velocity = new Vector2(velocityX, characterRigidbody.velocity.y);
    }

    void LimitInAirVelocity()
    {
        float velocityX = Mathf.Clamp(characterRigidbody.velocity.x, -maxAirVelocity.x, maxAirVelocity.x);
        float velocityY = Mathf.Clamp(characterRigidbody.velocity.y, -maxAirVelocity.y, maxAirVelocity.y);

        characterRigidbody.velocity = new Vector2(velocityX, velocityY);
    }

    #region Ground logic

    private void GroundCheck()
    {
        float boxCastAngle = 0f;
        float boxCastAddinationalSize = 0.01f;

        Vector2 boxCastSize = new Vector2(characterCollider.bounds.size.x, 0.1f);
        Vector2 boxCastStartPoint = new Vector2(characterCollider.bounds.center.x, characterCollider.bounds.center.y - characterCollider.bounds.extents.y);
        Vector2 boxCastDirection = Vector2.down;

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCastStartPoint, boxCastSize, boxCastAngle, boxCastDirection, groundCheckRayExtraLength, groundLayer);

        Color rayColor = Color.green;
        if (raycastHit.collider == true)
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(characterCollider.bounds.center + new Vector3(characterCollider.bounds.extents.x + boxCastAddinationalSize, 0), Vector2.down * (characterCollider.bounds.extents.y + groundCheckRayExtraLength), rayColor);
        Debug.DrawRay(characterCollider.bounds.center - new Vector3(characterCollider.bounds.extents.x + boxCastAddinationalSize, 0), Vector2.down * (characterCollider.bounds.extents.y + groundCheckRayExtraLength), rayColor);
        Debug.DrawRay(characterCollider.bounds.center - new Vector3(characterCollider.bounds.extents.x + boxCastAddinationalSize, characterCollider.bounds.extents.y + groundCheckRayExtraLength), Vector2.right * ((characterCollider.bounds.extents.x + boxCastAddinationalSize) * 2f), rayColor);

        if (raycastHit.collider)
        {
            if (!status.IsGrounded) CharacterLanded?.Invoke();

            status.IsGrounded = true;
            return;
        }
        status.IsGrounded = false;
    }

    private void GroundAngleCheck()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(characterCollider.bounds.center, Vector2.down, (characterCollider.bounds.size.y) + 1f, groundLayer);
        groundNormal = raycastHit.normal.x;

        if (groundNormal != 0f)
        {
            status.IsOnSlope = true;
        }
        else
        {
            status.IsOnSlope = false;
        }

        Quaternion rotation = Quaternion.Euler(0f, 0f, -raycastHit.normal.x * rotationSensivity);

        if (!status.IsGrounded)
        {
            rotation = Quaternion.Euler(0f, 0f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.fixedDeltaTime);
            return;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSmoothing);
    }

    #endregion Ground logic

    public void Jump()
    {
        if (status.IsGrounded)
        {
            Vector2 force = new Vector2(0f, jumpForce);
            characterRigidbody.AddForce(force, ForceMode2D.Impulse);
            status.IsJumping = true;
            CharacterJumped?.Invoke();
        }
    }

    public void CancelJump()
    {
        if (!status.IsJumping) return;

        float velocityY = characterRigidbody.velocity.y / 4f;

        characterRigidbody.velocity = new Vector2(characterRigidbody.velocity.x, velocityY);
        status.IsJumping = false;
    }

    void ApplyFallMultiplier()
    {
        if (characterRigidbody.velocity.y < 0f)
        {
            Vector2 newVelocity = new Vector2(characterRigidbody.velocity.x, characterRigidbody.velocity.y + (Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime));
            characterRigidbody.velocity = newVelocity;
        }
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        if (inputAxisX == 0) return;
        if (inputAxisX < 0)
        {
            scale.x = -1;
            transform.localScale = scale;
            status.IsFacingLeft = true;
        }
        else if (inputAxisX > 0)
        {
            scale.x = 1;
            transform.localScale = scale;
            status.IsFacingLeft = false;
        }
    }

    void FixedUpdate()
    {
        if (!spriteRenderer.isVisible) return;

        if (status.IsJumping)
        {
            if (characterRigidbody.velocity.y <= 0f)
            {
                status.IsJumping = false;
            }
        }

        Move();
        if (applyFallMultiplier) ApplyFallMultiplier();
        FlipSprite();
        GroundCheck();
        GroundAngleCheck();

        characterVelocity = characterRigidbody.velocity;
    }

    void OnDestroy()
    {
        CharacterLanded -= ReduceLandingVelocity;
    }
}