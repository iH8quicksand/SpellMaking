using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class MainMenuManager : MonoBehaviour
{
    // For Play Button
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    // For Quit Button
    public void QuitGame()
    {
        Application.Quit();
    }
}
