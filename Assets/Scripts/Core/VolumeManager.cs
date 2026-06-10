using UnityEngine;
using System;

public class VolumeManager : MonoBehaviour
{
    public float MasterVolume = 1.0f;
    public event Action OnVolumeChanged;

    public static VolumeManager Instance { get; private set; }

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

    public void SetVolume(float volume)
    {
        MasterVolume = volume;
        OnVolumeChanged?.Invoke();
    }
}
