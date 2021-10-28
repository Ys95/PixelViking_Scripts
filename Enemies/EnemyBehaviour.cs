using Pathfinding;
using System.Collections;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Player character")]
    [SerializeField] Rigidbody2D playerRigidbody;

    [Header("References")]
    [SerializeField] CharacterStatus status;
    [SerializeField] CharacterMovement movement;

    [Header("Componenets")]
    [SerializeField] Transform[] patrolPoints = new Transform[2];
    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] Collider2D enemyCollider;
    [SerializeField] CircleCollider2D playerDetector;
    [SerializeField] Rigidbody2D enemyRigidbody;
    [SerializeField] Seeker seeker;
    [SerializeField] Animator animator;

    [Space]
    [SerializeField] LayerMask obstaclesLayer;
    [SerializeField] Transform obstacleDetector;
    [SerializeField] float playerDetectionRange;
    [SerializeField] float nextWaypointDistance = 1f;
    [SerializeField] bool floatingEnemy;
    [SerializeField] bool patrollingEnemy;
    [SerializeField] bool ableToChasePlayer;

    bool facesHeightChange;
    bool canJump;

    Vector3 target;
    int currentWaypoint;
    bool isChasingPlayer = false;

    float distanceToA;
    float distanceToB;
    Vector2 direction;
    Vector2 force;

    float movementDirectionX;
    float movementDirectionY;
    private Path path;

    void Awake()
    {
        foreach (Transform patrolPoint in patrolPoints)
        {
            SpriteRenderer point = patrolPoint.GetComponent<SpriteRenderer>();
            if (point == null) continue;

            point.enabled = false;
        }

        GameObject player = GameObject.FindGameObjectWithTag(Tags.PLAYER);
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        canJump = true;
    }

    void Start()
    {
        if (patrollingEnemy)
        {
            target = patrolPoints[1].position;
        }

        if (ableToChasePlayer)
        {
            playerDetector.radius = playerDetectionRange;
        }
        else
        {
            playerDetector.enabled = false;
        }

        InvokeRepeating(nameof(UpdatePath), 0f, 0.2f);
        //InvokeRepeating(nameof(CalculateDirection), 0f, 0.1f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (isChasingPlayer)
            {
                target = playerRigidbody.transform.position;
            }
            seeker.StartPath(enemyRigidbody.position, target, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void MoveToNextPatrolPoint()
    {
        distanceToA = Vector2.Distance(enemyRigidbody.position, patrolPoints[0].position);
        distanceToB = Vector2.Distance(enemyRigidbody.position, patrolPoints[1].position);
        if (distanceToA > distanceToB)
        {
            target = patrolPoints[0].transform.position;
        }
        else
        {
            target = patrolPoints[1].transform.position;
        }
        KillHorizontalVelocity();
    }

    void StartChasingPlayer()
    {
        isChasingPlayer = true;
        target = playerRigidbody.transform.position;
        playerDetectionRange *= 3;
        playerDetector.radius = playerDetectionRange;
    }

    void StopChasingPlayer()
    {
        isChasingPlayer = false;
        target = patrolPoints[0].position;
        playerDetectionRange /= 3;
        playerDetector.radius = playerDetectionRange;
    }

    void CalculateDirection()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (!isChasingPlayer)
            {
                Invoke(nameof(MoveToNextPatrolPoint), 0.5f);
            }
            return;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - enemyRigidbody.position).normalized;

        StartCoroutine(ChangeDirectionX(direction.x));

        if (direction.y > 0f && floatingEnemy)
        {
            movementDirectionY = 1f;
        }
        else
        {
            movementDirectionY = -1f; //gravity will pull enemy down, no need for addinational force
        }

        float distance = Vector2.Distance(enemyRigidbody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    IEnumerator ChangeDirectionX(float directionX)
    {
        yield return new WaitForSeconds(0.5f);

        if (directionX > 0f && direction.x > 0f)
        {
            movementDirectionX = 1f;
        }
        else if (directionX < 0f && direction.x < 0f)
        {
            movementDirectionX = -1f;
        }
        yield return null;
    }

    void DetectHeightChange()
    {
        float rayCastDirectionX;
        float offset = 0.01f;
        Vector2 rayCastDirection;

        if (movementDirectionX > 0)
        {
            rayCastDirectionX = -1f;
            rayCastDirection = Vector2.right;
        }
        else
        {
            rayCastDirectionX = 1f;
            rayCastDirection = Vector2.left;
        }
        offset *= rayCastDirectionX;

        Vector2 raycastPoint = new Vector2((enemyCollider.bounds.center.x - offset - (enemyCollider.bounds.size.x / 2) * rayCastDirectionX),
            enemyCollider.bounds.center.y + Mathf.Abs(offset) - (enemyCollider.bounds.size.y / 2)); //ray will be cast from collider bottom corner

        RaycastHit2D raycastHit = Physics2D.Raycast(raycastPoint, rayCastDirection, 0.15f, obstaclesLayer);

        Color rayColor = Color.blue;
        if (raycastHit.collider == true)
        {
            rayColor = Color.magenta;
        }

        Debug.DrawRay(raycastPoint, rayCastDirection * (0.15f), rayColor);

        facesHeightChange = raycastHit.collider;
    }

    public void MovementEnabled(bool move)
    {
        status.CanMove = move;
    }

    public void KillHorizontalVelocity()
    {
        Vector2 newVelocity = new Vector2(0f, enemyRigidbody.velocity.y);
        enemyRigidbody.velocity = newVelocity;
    }

    public void KillVerticalVelocity()
    {
        Vector2 newVelocity = new Vector2(enemyRigidbody.velocity.x, 0f);
        enemyRigidbody.velocity = newVelocity;
    }

    IEnumerator JumpCooldown()
    {
        float timer = Random.Range(0.2f, 1f);
        canJump = false;

        while (timer > 0f)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
        canJump = true;
    }

    IEnumerator FlyUp()
    {
        int ticks = 10;

        while (ticks > 0)
        {
            enemyRigidbody.AddForce(new Vector2(0f, 3f));
            yield return new WaitForFixedUpdate();
            ticks--;
        }
    }

    void FixedUpdate()
    {
        if (!enemySprite.isVisible) return;

        CalculateDirection();
        DetectHeightChange();
        animator.SetBool("isGrounded", status.IsGrounded);

        if (!status.IsGrounded && !floatingEnemy)
        {
            movementDirectionX /= 3f;
        }

        if (status.IsGrounded && floatingEnemy)
        {
            StartCoroutine(FlyUp());
        }

        if (!status.CanMove || status.IsAttacking)
        {
            movementDirectionX = 0f;
            movementDirectionY = 0f;
        }

        if (status.CanMove)
        {
            if (facesHeightChange && canJump)
            {
                movement.Jump();
                animator.SetTrigger("jump");
                StartCoroutine(JumpCooldown());
            }
        }

        movement.InputAxisX = movementDirectionX; //this sets input value in movements script which allows enemy to move
        movement.InputAxisY = movementDirectionY;

        if (animator == null) return;
        if (movementDirectionX == -1f || movementDirectionX == 1f)
        {
            animator.SetBool("inputX", true);
            return;
        }
        animator.SetBool("inputX", false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tags.PLAYER) StartChasingPlayer();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Tags.PLAYER) StopChasingPlayer();
    }
}