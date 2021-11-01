using UnityEngine;
using System.Collections;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField] PlayerScriptReferences player;

    [Space]
    [Header("Componenents")]
    [SerializeField] BoxCollider2D playerCollider;
    [SerializeField] Rigidbody2D playerRb;

    [Space]
    [SerializeField] float wiggleStrength;
    [SerializeField] Vector3 grabDistance;
    [SerializeField] LayerMask grabableLayer;

    FixedJoint2D grabbedJoint;
    Rigidbody2D grabbedRb;
    GrabableChain grabbedObject;
    float inputAxisX;

    public delegate void OnPlayerGrab(bool grabbed);
    public static event OnPlayerGrab PlayerIsGrabbing;

    public float InputAxisX { get => inputAxisX; set => inputAxisX = value; }

    void Awake()
    {
        PlayerHealth.PlayerDamaged += StopGrabing;
    }

    public bool LookForGrabable()
    {
        Collider2D[] targetsHit = Physics2D.OverlapBoxAll(playerCollider.bounds.center, grabDistance, 0f, grabableLayer);
        bool found = false;
        float shortestDistance = float.MaxValue;
        Collider2D closestCollider = null;

        foreach (Collider2D target in targetsHit)
        {
            found = true;
            Debug.Log("In grab range: " + target.name);
            float currentDistance = Vector2.Distance(playerCollider.transform.position, target.transform.position);
            if (currentDistance < shortestDistance)
            {
                shortestDistance = currentDistance;
                closestCollider = target;
            }
        }

        if (found)
        {
            grabbedJoint = closestCollider.GetComponent<FixedJoint2D>();
            grabbedRb = closestCollider.GetComponent<Rigidbody2D>();
            grabbedObject = closestCollider.GetComponent<GrabableChain>();

            Vector2 newPos = new Vector2(grabbedJoint.transform.position.x, grabbedJoint.transform.position.y + 0.1f);

            transform.position = newPos;

            grabbedJoint.connectedBody = playerRb;
            grabbedJoint.enabled = true;

            GrabMode(true);
            return true;
        }
        return false;
    }

    public void StopGrabing()
    {
        if (grabbedJoint != null && grabbedRb != null)
        {
            grabbedJoint.enabled = false;
            grabbedJoint.connectedBody = null;
            GrabMode(false);

            AddVelocityFromChain(grabbedRb.velocity);
            grabbedObject = null;
            grabbedRb = null;
            grabbedJoint = null;
        }
    }

    void AddVelocityFromChain(Vector2 chainVelocity)
    {
        Vector2 addedVelocity = new Vector2(chainVelocity.x * 1.5f, chainVelocity.y / 2f);
        playerRb.AddForce(addedVelocity, ForceMode2D.Impulse);
    }


    void GrabMode(bool activate)
    {
        if (activate)
        {
            player.Input.ActivateGrabActionMap();
            player.Status.IsGrabing = true;
            player.Animations.TriggerGrab(true);
            PlayerIsGrabbing?.Invoke(true);
            return;
        }
        PlayerIsGrabbing?.Invoke(false);
        player.Status.IsGrabing = false;
        player.Animations.TriggerGrab(false);
        player.Input.ActivateGroundActionMap();
    }

    public void Wiggle()
    {
        Vector2 force = new Vector2(wiggleStrength * InputAxisX, 0f);
        grabbedRb.AddForce(force);
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        if (inputAxisX < 0)
        {
            scale.x = -1;
            transform.localScale = scale;
            player.Status.IsFacingLeft = true;
        }
        else if (inputAxisX > 0)
        {
            scale.x = 1;
            transform.localScale = scale;
            player.Status.IsFacingLeft = true;
        }
    }

    void FixedUpdate()
    {
        if (grabbedRb != null)
        {
            Wiggle();
            FlipSprite();
        }
    }

    void OnDisable()
    {
        PlayerHealth.PlayerDamaged -= StopGrabing;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(playerCollider.bounds.center, grabDistance);
    }
}