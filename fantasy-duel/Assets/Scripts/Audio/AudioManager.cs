using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource[] sources;
    [SerializeField] private AudioClip[] clips;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("AudioManager is NULL.");
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        sources = GetComponents<AudioSource>();

        Play(Audio.Soundtrack, Clip.Menu, true);

        if (SceneManager.GetActiveScene().buildIndex == 1)
            Play(Audio.Soundtrack, Clip.Match, true);
    }

    public void Play(Audio audio, Clip clip, bool isLooping)
    {
        sources[(int)audio].clip = clips[(int)clip];
        sources[(int)audio].volume = 1f;
        sources[(int)audio].Play();
        sources[(int)audio].loop = isLooping;
    }

    public void UpdateVolume(Audio audio, float value)
    {
        sources[(int)audio].volume += value;
    }
}
