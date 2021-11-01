using UnityEngine;

public class BouncyObject : MonoBehaviour
{
    [SerializeField] Vector2 bounceStrength;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != Tags.PLAYER) return;

        Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        rb.AddForce(bounceStrength, ForceMode2D.Impulse);
        SoundManager.Instance.PlaySound(SoundCategory.Misc, SoundEffect.misc_Bounce, collision.transform);
    }
}