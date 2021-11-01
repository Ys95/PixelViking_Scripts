using System.Collections.Generic;
using UnityEngine;

public enum SoundCategory
{
    Player,
    Ambient,
    Enemy,
    Misc,
    BGM,
    Unsorted,
}

public enum SoundEffect
{
    none,

    player_Steps,
    player_Attack,
    player_ComboAttack,
    player_AttackHit,
    player_Jump,
    player_Hurt,
    player_Death,
    player_ComboReady,

    beholder_projectile_spawn,
    beholder_projectile_hit,

    enemy_Death,

    misc_ItemPickedUp,
    misc_SpawnPointActivated,

    BGM_MainMenu,
    BGM, Level1,

    Unsorted,

    player_BuffUse,
    player_Healing,

    misc_Interaction,
    misc_TutorialText,
    misc_Bounce,

    player_Landing,

    misc_ChainCatch,
    misc_TimeSlowDown,
}

[CreateAssetMenu(fileName = "audioAssetsDatabase_name", menuName = "Resources Database/AudioAssetDatabase")]
public class AudioAssetsDatabase : ScriptableObject
{
    [SerializeField] List<AudioClipAsset> playerAudioClips;
    [SerializeField] List<AudioClipAsset> miscAudioClips;
    [SerializeField] List<AudioClipAsset> enemyAudioClips;
    [SerializeField] List<AudioClipAsset> bgmAudioClips;
    [SerializeField] List<AudioClipAsset> ambientAudioClips;
    [SerializeField] List<AudioClipAsset> unsortedAudioClips;

    Dictionary<SoundEffect, AudioClipAsset> playerAudioClipsDictionary;
    Dictionary<SoundEffect, AudioClipAsset> miscAudioClipsDictionary;
    Dictionary<SoundEffect, AudioClipAsset> enemyAudioClipsDictionary;
    Dictionary<SoundEffect, AudioClipAsset> ambientAudioClipsDictionary;
    Dictionary<SoundEffect, AudioClipAsset> bgmAudioClipsDictionary;

    //intended to be called manually from inspector
    [ContextMenu("Update Database")]
    public void RefreshDatabase()
    {
        List<AudioClip> allClips = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio"));

        List<AudioClipAsset> clipsInDB
            = new List<AudioClipAsset>(playerAudioClips.Count + miscAudioClips.Count + enemyAudioClips.Count
            + bgmAudioClips.Count + ambientAudioClips.Count + unsortedAudioClips.Count);

        clipsInDB.AddRange(playerAudioClips);
        clipsInDB.AddRange(miscAudioClips);
        clipsInDB.AddRange(enemyAudioClips);
        clipsInDB.AddRange(bgmAudioClips);
        clipsInDB.AddRange(ambientAudioClips);
        clipsInDB.AddRange(unsortedAudioClips);

        foreach (AudioClipAsset clipAsset in clipsInDB)
        {
            if (allClips.Contains(clipAsset.Audio))
            {
                allClips.Remove(clipAsset.Audio);
            }
        }

        SortClips(allClips);
    }

    public void RefreshDictionary()
    {
        List<AudioClipAsset> allClipAssets = new List<AudioClipAsset>();

        allClipAssets.AddRange(playerAudioClips);
        allClipAssets.AddRange(miscAudioClips);
        allClipAssets.AddRange(enemyAudioClips);
        allClipAssets.AddRange(bgmAudioClips);
        allClipAssets.AddRange(ambientAudioClips);

        playerAudioClipsDictionary = new Dictionary<SoundEffect, AudioClipAsset>();
        enemyAudioClipsDictionary = new Dictionary<SoundEffect, AudioClipAsset>();
        miscAudioClipsDictionary = new Dictionary<SoundEffect, AudioClipAsset>();
        bgmAudioClipsDictionary = new Dictionary<SoundEffect, AudioClipAsset>();
        ambientAudioClipsDictionary = new Dictionary<SoundEffect, AudioClipAsset>();

        foreach (AudioClipAsset clipAsset in allClipAssets)
        {
            Dictionary<SoundEffect, AudioClipAsset> dict = FindCorrectDictionary(clipAsset.SoundCategory);

            if (clipAsset.SoundEffectType != SoundEffect.Unsorted && dict != null)
            {
                dict.Add(clipAsset.SoundEffectType, clipAsset);
            }
        }
    }

    void SortClips(List<AudioClip> audioClips)
    {
        foreach (AudioClip clip in audioClips)
        {
            string clipName = clip.name;
            SoundCategory category;
            //correct list will be  decided during sorting
            List<AudioClipAsset> list;

            #region Sorting If statements

            if (clipName.Contains("player_"))
            {
                category = SoundCategory.Player;
                list = playerAudioClips;
            }
            else if (clipName.Contains("misc_"))
            {
                category = SoundCategory.Misc;
                list = miscAudioClips;
            }
            else if (clipName.Contains("enemy_"))
            {
                category = SoundCategory.Enemy;
                list = enemyAudioClips;
            }
            else if (clipName.Contains("ambient_"))
            {
                category = SoundCategory.Ambient;
                list = ambientAudioClips;
            }
            else if (clipName.Contains("bgm_"))
            {
                category = SoundCategory.BGM;
                list = bgmAudioClips;
            }
            else
            {
                category = SoundCategory.Unsorted;
                list = unsortedAudioClips;
            }

            #endregion Sorting If statements

            AudioClipAsset clipAsset = new AudioClipAsset(category, clip);

            if (list.Contains(clipAsset)) return;
            list.Add(clipAsset);
        }
    }

    public AudioClipAsset FindAudioClip(SoundCategory category, SoundEffect effectType)
    {
        Dictionary<SoundEffect, AudioClipAsset> dict = FindCorrectDictionary(category);

        if (dict.ContainsKey(effectType))
        {
            return dict[effectType];
        }
        Debug.LogWarning(category + " sound " + effectType + " not found.");
        return null;
    }

    Dictionary<SoundEffect, AudioClipAsset> FindCorrectDictionary(SoundCategory category)
    {
        Dictionary<SoundEffect, AudioClipAsset> dict;

        switch (category)
        {
            case SoundCategory.Player:
                dict = playerAudioClipsDictionary;
                break;

            case SoundCategory.Enemy:
                dict = enemyAudioClipsDictionary;
                break;

            case SoundCategory.BGM:
                dict = bgmAudioClipsDictionary;
                break;

            case SoundCategory.Ambient:
                dict = ambientAudioClipsDictionary;
                break;

            case SoundCategory.Misc:
                dict = miscAudioClipsDictionary;
                break;

            default:
                dict = null;
                break;
        }
        return dict;
    }
}

[System.Serializable]
public class AudioClipAsset
{
    [SerializeField] SoundCategory soundCategory;

    [SerializeField] SoundEffect soundEffectType;

    [SerializeField] AudioClip audio;
    [Range(0f, 3f)] [SerializeField] protected float volume = 1f;
    [Range(0f, 3f)] [SerializeField] protected float pitch = 1f;

    public SoundCategory SoundCategory { get => soundCategory; }
    public SoundEffect SoundEffectType { get => soundEffectType; }
    public AudioClip Audio { get => audio; }
    public float Volume { get => volume; }
    public float Pitch { get => pitch; }

    public AudioClipAsset(SoundCategory category, AudioClip clip)
    {
        soundCategory = category;
        soundEffectType = SoundEffect.Unsorted;
        audio = clip;
    }
}