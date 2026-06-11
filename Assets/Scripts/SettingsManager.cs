using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

    // VolumeSlider calls this function to set the volume
    public void SetVolume(float volume)
    {
        VolumeManager.Instance.SetVolume((80f+volume)/80f);
    }

    // Togglable FullScreen Setting
    public void SetFullScreen(bool isFullScreen)
    {
        ButtonAudioManager.Instance.PlayClick();
        Screen.fullScreen = isFullScreen;
    }
}
