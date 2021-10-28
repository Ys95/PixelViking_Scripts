using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    ParticleSystem particle;

    public void Play(ParticleSystem particleArg, Transform location)
    {
        particle = particleArg;
        transform.position = location.position;
        particle.Play();
        Invoke("DisableThisGameobject", particle.main.duration);
    }

    void DisableThisGameobject()
    {
        gameObject.SetActive(false);
    }
}