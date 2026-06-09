using UnityEngine;

//public class PauseManager : MonoBehaviour
//{
//    public GameObject pauseMenuUI;
//    public bool isPaused = false;
    
//    void OnEnable()
//    {
//        EventBus.Instance.OnPauseToggled += TogglePause;
//    }
//    void OnDisable()
//    {
//        EventBus.Instance.OnPauseToggled -= TogglePause;
//    }

//    void TogglePause()
//    {
//        if (GameManager.Instance.state == GameManager.GameState.GAMEOVER)
//            return;

//        isPaused = !isPaused;

//        Time.timeScale = isPaused ? 0f : 1f;

//        pauseMenuUI.SetActive(isPaused);
//    }

//}
