using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class GrabableChain : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] CircleCollider2D handleArea;
    [SerializeField] Rigidbody2D handleRb;
    [SerializeField] FixedJoint2D joint;
    [SerializeField] Rigidbody2D[] chainLinksRb;

    [Space]
    [SerializeField] Light2D handleLight;

    public delegate void OnPlayerInRange();
    public static event OnPlayerInRange PlayerIsInGrabRange;

    bool isGrabbedByPlayer;

    public Rigidbody2D HandleRb { get => handleRb; private set => handleRb = value; }
    public FixedJoint2D Joint { get => joint; }

    void Awake()
    {
        PlayerGrab.PlayerIsGrabbing += GotGrabbed;
    }

    public void KillVelocity()
    {
        HandleRb.velocity = Vector2.zero;
        handleRb.angularVelocity = 0f;
        foreach (Rigidbody2D link in chainLinksRb)
        {
            link.velocity = Vector2.zero;
            link.angularVelocity = 0f;
        }
    }

    public void GotGrabbed(bool grabbed)
    {
        if(grabbed)
        {
            SoundManager.Instance.PlaySound(SoundCategory.Misc, SoundEffect.misc_ChainCatch, transform);
            isGrabbedByPlayer = true;
            return;
        }
        isGrabbedByPlayer = false;

        Invoke(nameof(KillVelocity), 0.15f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;
        handleLight.enabled = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;
        if (isGrabbedByPlayer) return;
        handleLight.enabled = false;
    }

    void OnDisable()
    {
        PlayerGrab.PlayerIsGrabbing -= GotGrabbed;
    }
}