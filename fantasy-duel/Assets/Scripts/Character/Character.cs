using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private ParticleSystem[] particles;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(CharacterAnimations animations)
    {
        animator.SetTrigger(animations.ToString());
    }

    public void PlayParticles(CharacterParticles characterParticles)
    {
        particles[(int)characterParticles].Play();
    }

    public void StopParticles(CharacterParticles characterParticles)
    {
        particles[(int)characterParticles].Stop();
    }
}
