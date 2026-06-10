using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    public static ButtonAudio Instance;

    public AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        Instance = this;
    }

    public void PlayClick()
    {
        audioSource.PlayOneShot(clickSound);
    }
}