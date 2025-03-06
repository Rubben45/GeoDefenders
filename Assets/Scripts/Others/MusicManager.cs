using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip level1to3Music;
    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Ensure the music keeps playing if it stops for any reason
        if (!musicSource.isPlaying)
        {
            PlayMusicBasedOnScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicBasedOnScene(scene.buildIndex);
    }

    void PlayMusicBasedOnScene(int sceneIndex)
    {
        AudioClip clipToPlay = null;

        if (sceneIndex == 0) // Main Menu
        {
            clipToPlay = mainMenuMusic;
        }
        else if (sceneIndex == 1 || sceneIndex == 2 || sceneIndex == 3) // Levels 1, 2, and 3
        {
            clipToPlay = level1to3Music;
        }
        // Add more conditions here for other levels or scenes if necessary

        if (clipToPlay != null && musicSource.clip != clipToPlay)
        {
            musicSource.clip = clipToPlay;
            musicSource.loop = true; // Set the music to loop
            musicSource.Play();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
