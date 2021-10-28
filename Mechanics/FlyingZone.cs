using System.Collections;
using UnityEngine;

public class FlyingZone : MonoBehaviour
{
    [SerializeField] float flyingForce;

    Rigidbody2D playerRb;
    bool playerInZone;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;

        playerRb = collision.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        playerInZone = true;
        playerRb.drag *= 2f;
        StartCoroutine(Flying(playerRb));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;

        playerRb.drag /= 2f;
        playerInZone = false;
    }

    IEnumerator Flying(Rigidbody2D rb)
    {
        while (playerInZone)
        {
            rb.AddForce(new Vector2(0f, flyingForce));
            yield return new WaitForFixedUpdate();
        }
        playerRb = null;
    }
}