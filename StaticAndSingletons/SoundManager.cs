using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    private static SoundManager instance = null;
    public static SoundManager Instance { get => instance; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        LoadVolumeSettings();
        PlayerHealth.PlayerIsDead += MuffleSound;
    }

    #endregion Singleton

    [SerializeField] AudioSource bgmPlayer;
    [SerializeField] AudioLowPassFilter soundMuffler;

    float globalMusicVolume;
    float globalSoundEffectsVolume;

    public void LoadVolumeSettings()
    {
        globalMusicVolume = PlayerSettings.MusicVolume.Load() * PlayerSettings.DEFAULT_MUSIC_VOLUME;
        globalSoundEffectsVolume = PlayerSettings.SoundEffectsVolume.Load();

        bgmPlayer.volume = globalMusicVolume;
    }

    public void PlaySound(SoundCategory category, SoundEffect soundEffect, Transform position)
    {
        if (soundEffect == SoundEffect.none) return;

        GameObject obj = ObjectPool.Instance.GetPooledObject(Tags.SOUND_OBJECT);
        SoundObjectScript soundObject = obj.GetComponent<SoundObjectScript>();

        AudioClipAsset clip = Databases.Instance.AudioDB.FindAudioClip(category, soundEffect);
        if (clip == null) return;

        obj.transform.position = position.position;
        obj.SetActive(true);
        float volume = clip.Volume * globalSoundEffectsVolume;
        soundObject.Play(clip, volume);
    }

    public void PlaySound(AudioClipAsset clip, Vector2 position)
    {
        GameObject obj = ObjectPool.Instance.GetPooledObject(Tags.SOUND_OBJECT);
        SoundObjectScript soundObject = obj.GetComponent<SoundObjectScript>();

        obj.transform.position = position;
        obj.SetActive(true);
        float volume = clip.Volume * globalSoundEffectsVolume;
        soundObject.Play(clip, volume);
    }

    void MuffleSound()
    {
        soundMuffler.enabled = true;
    }

    void OnDestroy()
    {
        PlayerHealth.PlayerIsDead -= MuffleSound;
    }
}