using UnityEngine;
using UnityEngine.SceneManagement;

public class CardParticlesManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            RotateParticles();
        }
    }

    private void RotateParticles()
    {
        foreach (var particle in particles)
        {
            if(!particle.CompareTag("NonRotatableParticles"))
            {
                particle.transform.localRotation = Quaternion.Euler(90, 0, 0);
            }
        }
    }

    public void Play(CardParticles cardParticles)
    {
        particles[(int)cardParticles].Play();
    }

    public void Stop(CardParticles cardParticles)
    {
        particles[(int)cardParticles].Stop();
    }
}
