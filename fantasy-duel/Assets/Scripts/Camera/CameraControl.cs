using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private bool HasKeyBeenPressed = false;

    [SerializeField] private Transform destination;
    [SerializeField] private float speed = 3000f;

    [SerializeField] private GameObject canvasLogo;
    [SerializeField] private ParticleSystem logoParticles;

    private void Update()
    {
        if (Input.anyKeyDown && !HasKeyBeenPressed)
        {
            HasKeyBeenPressed = true;
            HideLogo();
            AudioManager.Instance.Play(Audio.SoundEffects, Clip.Turn, false);
        }

        if (HasKeyBeenPressed)
            Move();
    }

    private void HideLogo()
    {
        canvasLogo.SetActive(false);
        logoParticles.Play();
    }

    private void Move()
    {
        if (transform.position != destination.position)
            transform.position = Vector3.MoveTowards(transform.position, destination.position, speed * Time.deltaTime);
    }
}
