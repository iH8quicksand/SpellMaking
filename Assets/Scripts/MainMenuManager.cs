using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class MainMenuManager : MonoBehaviour
{
    // For Play Button
    public void LoadScene(string sceneName)
    {
        ButtonAudioManager.Instance.PlayClick();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // For Quit Button
    public void QuitGame()
    {
        ButtonAudioManager.Instance.PlayClick();
        Application.Quit();
    }
}
