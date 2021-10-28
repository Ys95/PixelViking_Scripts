using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] AudioAssetsDatabase audioDB;

    PlayerSound playerSteps;
    PlayerSound playerJump;
    PlayerSound playerHurt;
    PlayerSound playerLanded;
    PlayerSound playerAttack;
    PlayerSound playerAttackHit;
    PlayerSound playerComboReady;
    PlayerSound playerComboAttack;
    PlayerSound playerDeath;
    PlayerSound playerBuffUse;
    PlayerSound playerHealing;

    #region Getters

    public PlayerSound PlayerSteps { get => playerSteps; }
    public PlayerSound PlayerJump { get => playerJump; }
    public PlayerSound PlayerHurt { get => playerHurt; }
    public PlayerSound PlayerLanded { get => playerLanded; }
    public PlayerSound PlayerAttack { get => playerAttack; }
    public PlayerSound PlayerAttackHit { get => playerAttackHit; }
    public PlayerSound PlayerComboReady { get => playerComboReady; }
    public PlayerSound PlayerComboAttack { get => playerComboAttack; }
    public PlayerSound PlayerDeath { get => playerDeath; }
    public PlayerSound PlayerBuffUse { get => playerBuffUse; }
    public PlayerSound PlayerHealing { get => playerHealing; }

    #endregion Getters

    public struct PlayerSound
    {
        AudioClipAsset audio;

        public AudioClipAsset Audio { get => audio; set => audio = value; }

        public void Play(Vector2 pos)
        {
            SoundManager.Instance.PlaySound(audio, pos);
        }
    }

    void Awake()
    {
        GetClips();
    }

    public void GetClips()
    {
        audioDB.RefreshDictionary();

        playerSteps.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_Steps);
        playerJump.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_Jump);
        playerHurt.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_Hurt);
        playerLanded.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_Landing);
        playerAttack.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_Attack);
        playerAttackHit.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_AttackHit);
        playerComboReady.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_ComboReady);
        playerComboAttack.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_ComboAttack);
        playerDeath.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_Death);
        playerBuffUse.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_BuffUse);
        playerHealing.Audio = audioDB.FindAudioClip(SoundCategory.Player, SoundEffect.player_Healing);
    }
}