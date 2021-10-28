using UnityEngine;

public class LowGravityZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;

        Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
        if (rigidbody == null) return;

        rigidbody.gravityScale *= 0.5f;
        rigidbody.drag *= 5f;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;

        Rigidbody2D rigidbody = collision.GetComponent<Rigidbody2D>();
        if (rigidbody == null) return;

        rigidbody.gravityScale /= 0.5f;
        rigidbody.drag /= 5f;
    }
}