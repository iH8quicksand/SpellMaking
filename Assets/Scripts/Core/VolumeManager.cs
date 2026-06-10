using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public float MasterVolume = 1.0f;

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
}
