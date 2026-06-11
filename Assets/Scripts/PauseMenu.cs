using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    public void TogglePaused() // Escape key calls this (through playercontroller)
    {
        if (GameIsPaused) Resume();
        else Pause();
    }

    public void Resume()
    {
        ButtonAudioManager.Instance.PlayClick();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.state = GameManager.GameState.INWAVE;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.Instance.state = GameManager.GameState.PAUSED;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        ButtonAudioManager.Instance.PlayClick();
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Quit()
    {
        ButtonAudioManager.Instance.PlayClick();
        Application.Quit();
    }
}
