using UnityEngine;

public class SoundObjectScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject player;
    [SerializeField] AudioSource source;

    [Space]
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Play(AudioClipAsset clipAsset, float volume)
    {
        float dist = Vector2.Distance(transform.position, player.transform.position);

        float soundVolume = volume;

        if (dist < minDistance)
        {
            soundVolume *= 1f;
        }
        if (dist > maxDistance)
        {
            soundVolume *= 0f;
        }
        else
        {
            soundVolume *= 0.5f;
        }

        source.clip = clipAsset.Audio;
        source.volume = soundVolume;
        source.pitch = clipAsset.Pitch;

        source.Play();

        Invoke("DisableGameobject", clipAsset.Audio.length + 0.01f);
    }

    void DisableGameobject()
    {
        gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        Vector2 size = new Vector2(maxDistance, maxDistance);

        Gizmos.DrawWireCube(transform.position, size);
    }
}