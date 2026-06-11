using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    AudioSource player;
    AudioClip music_menu;
    AudioClip music_game;

    public static MusicManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<AudioSource>();
        music_menu = Resources.Load<AudioClip>("Audio/music_menu");
        music_game = Resources.Load<AudioClip>("Audio/music_game");
        SceneManager.activeSceneChanged += UpdateClip;
        VolumeManager.Instance.OnVolumeChanged += ChangeVolume;
    }

    // Update is called once per frame
    private void UpdateClip(Scene _, Scene newScene)
    {
        if (newScene.name == "Main")
        {
            player.clip = music_game;
            player.Play();
        }
        else if (player.clip != music_menu)
        {
            player.clip = music_menu;
            player.Play();
        }
    }

    private void ChangeVolume()
    {
        player.volume = VolumeManager.Instance.MasterVolume;
    }
}
