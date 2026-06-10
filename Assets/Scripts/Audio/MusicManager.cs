using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    AudioSource player;
    AudioClip music1;
    AudioClip music2;

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
        music1 = Resources.Load<AudioClip>("Audio/music1");
        music2 = Resources.Load<AudioClip>("Audio/music2");
        SceneManager.activeSceneChanged += UpdateClip;
        VolumeManager.Instance.OnVolumeChanged += ChangeVolume;
    }

    // Update is called once per frame
    private void UpdateClip(Scene _, Scene newScene)
    {
        if (newScene.name == "Main")
        {
            player.clip = music2;
            player.Play();
        }
        else if (player.clip != music1)
        {
            player.clip = music1;
            player.Play();
        }
    }

    private void ChangeVolume()
    {
        player.volume = VolumeManager.Instance.MasterVolume;
    }
}
