using UnityEngine;

public class ButtonAudioManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clickSound;
    public static ButtonAudioManager Instance { get; private set; }
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

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound, VolumeManager.Instance.MasterVolume);
    }
}