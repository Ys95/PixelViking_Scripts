using UnityEngine;

/// <summary>
/// Handles animation events and normal events.
/// </summary>
public class PlayerEventsHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerScriptReferences player;
    [SerializeField] ParticleSystem stepsParticle;
    [SerializeField] ParticleSystem landingParticle;

    void Awake()
    {
        player.Movement.CharacterLanded += PlayerLanded;
        player.Movement.CharacterJumped += PlayerJumped;
    }

    public void StartAttack() => player.Attack.PerformAttack(false);

    void PlayerLanded()
    {
        landingParticle.Play();
        player.Sounds.PlayerLanded.Play(transform.position);
    }

    public void PlayerMadeStep()
    {
        player.Sounds.PlayerSteps.Play(transform.position);
        stepsParticle.Play();
    }

    void PlayerJumped()
    {
        player.Sounds.PlayerJump.Play(transform.position);
    }

    void OnDestroy()
    {
        player.Movement.CharacterLanded -= PlayerLanded;
        player.Movement.CharacterJumped -= PlayerJumped;
    }
}