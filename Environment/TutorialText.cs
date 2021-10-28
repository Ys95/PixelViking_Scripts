using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] TextMeshPro text;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        text.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;
        text.enabled = true;
        SoundManager.Instance.PlaySound(SoundCategory.Misc, SoundEffect.misc_TutorialText, transform);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != Tags.PLAYER) return;
        text.enabled = false;
    }
}