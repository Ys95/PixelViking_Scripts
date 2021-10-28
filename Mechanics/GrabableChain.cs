using UnityEngine;

public class GrabableChain : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] Rigidbody2D handleRb;
    [SerializeField] FixedJoint2D joint;
    [SerializeField] Rigidbody2D[] chainLinksRb;

    public Rigidbody2D HandleRb { get => handleRb; private set => handleRb = value; }
    public FixedJoint2D Joint { get => joint; }

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
}