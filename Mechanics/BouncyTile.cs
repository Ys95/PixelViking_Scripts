using UnityEngine;

public class BouncyTile : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != Tags.PLAYER) return;
        SoundManager.Instance.PlaySound(SoundCategory.Misc, SoundEffect.misc_Bounce, collision.transform);
    }
}