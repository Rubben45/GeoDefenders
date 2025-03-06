using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip cannonTowerAttack;
    [SerializeField] private AudioClip templeTowerAttack;
    [SerializeField] private AudioClip mageTowerAttack;

    public Dictionary<string, AudioClip> soundEffects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        soundEffects = new Dictionary<string, AudioClip>();
    }
    void Start()
    {
        AddSoundEffect("Click", clickClip);
        AddSoundEffect("Cannon", cannonTowerAttack);
        AddSoundEffect("Temple", templeTowerAttack);
        AddSoundEffect("Mage", mageTowerAttack);
    }

    public void AddSoundEffect(string name, AudioClip clip)
    {
        soundEffects[name] = clip;
    }

    public void PlaySoundEffect(string name)
    {
        if (soundEffects.TryGetValue(name, out AudioClip clip))
        {
            sfxAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + name);
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }
}
