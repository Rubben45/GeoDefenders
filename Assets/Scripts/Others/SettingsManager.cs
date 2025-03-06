using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        // Set initial slider values based on current mixer settings
        float sfxVolume;
        mixer.GetFloat("SFXVolume", out sfxVolume);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);

        float musicVolume;
        mixer.GetFloat("MusicVolume", out musicVolume);
        musicSlider.value = Mathf.Pow(10, musicVolume / 20);
    }

    public void OnSFXVolumeChanged(float SFXvalue)
    {
        SetSFXVolume(SFXvalue);
    }

    private void SetSFXVolume(float value)
    {
        Debug.Log($"Setting SFX Volume: {value}, Logarithmic: {Mathf.Log10(value) * 20}");
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    public void OnMusicVolumeChanged(float musicValue)
    {
        SetMusicVolume(musicValue);
    }

    private void SetMusicVolume(float value)
    {
        Debug.Log($"Setting Music Volume: {value}, Logarithmic: {Mathf.Log10(value) * 20}");
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}
