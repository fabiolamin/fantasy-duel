using UnityEngine;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        SetVolume();
    }

    private void Update()
    {
        AudioListener.volume = slider.value;
    }

    private void SetVolume()
    {
        float volume;

        if (PlayerPrefs.GetInt("IsTheFirstTime") == 0)
        {
            volume = 1f;
        }
        else
        {
            volume = PlayerPrefs.GetFloat("AudioVolume");
        }

        slider.value = volume;
        AudioListener.volume = volume;
    }

    public void SaveChanges()
    {
        PlayerPrefs.SetFloat("AudioVolume", slider.value);
        PlayerPrefs.SetInt("IsTheFirstTime", 1);
        AudioListener.volume = slider.value;
    }
}
